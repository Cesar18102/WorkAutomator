AP_CONFIG = { ssid = "esp8266_detector", pwd = "11111111", auth = wifi.WPA2_PSK, max = 1 }
IP_CONFIG = { ip = "192.168.1.1", netmask = "255.255.255.252", gateway = "192.168.1.1" }
PORT_LISTENING = 80

DETECTOR_ID = 9
DATA_PREFAB_ID = 7

SERVER_URL = "http://workautomatorback.azurewebsites.net/api/Detector/ProvideData"
APP_JSON_CONTENT_TYPE = 'Content-Type: application/json\r\n'

REQUEST_ERROR_DELAY = 1000

SEND_DATA_TIMER = nil
SEND_DATA_INTERVAL = 5000

function connect_ap(ssid, pwd, callback)
  print("connecting to "..ssid)
  wifi.setmode(wifi.STATION)

  sta_cnf = { ssid = ssid, pwd = pwd, auto = false }

  print(sta_cnf.ssid)
  print(sta_cnf.pwd)
    
  if(wifi.sta.config(sta_cnf)) then
    print("connected to "..ssid)
  else
    print("failed to connect to "..ssid.." wrong creds either ap is unreachable")
    node.restart()
    return
  end

  wifi.sta.connect(callback)
end

function creds_reciever(conn, data, callback)
  local creds = data:gmatch("ssid=.+")(0)
    
  if(creds == nil) then
    print("invalid creds recieved. rebooting.")
    conn:send("400")
    node.restart()
    return
  end

  ssid = creds:gmatch("ssid=([^&]+)")(0)
  pwd = creds:gmatch("pwd=(.+)")(0)

  if(pwd == nil) then pwd = "" end

  print("ssid parsed: "..ssid)
  print("pwd parsed: "..pwd)

  conn:send("200")
  conn:close()

  server:close()
  print("server closed")

  if(callback ~= nil) then
    callback(ssid, pwd)
  end
end

-----------------------------

function set_interval(function_to_repeat, interval)
  local timer = tmr.create()
  timer:register(
    interval, tmr.ALARM_AUTO,
    function(timer) 
      function_to_repeat()
    end
  )
  timer:start()
  return timer
end

function set_timeout(function_to_timeout, timeout)
  local timer = tmr.create()
  timer:register(
    timeout, tmr.ALARM_SINGLE,
    function(timer) 
      function_to_timeout();
    end
  )
  timer:start()
  return timer
end

------------------------

function build_uri(url, params)
  if params == nil then return url; end
  
  local params_string = "";
  for k, v in pairs(params) do
    params_string = params_string..k.."="..v.."&";
  end
  
  params_string = strsub(params_string, 1, strlen(params_string) - 1);
  if not params_string then
    return url.."?"..params_string;
  end
  
  return url;
end

function send_request(url, method, headers, body, success_callback, fatal_callback)
  http.request(
    url, method, headers, body, 
    function(code, data)
      if code < 0 then
        if fatal_callback == nil then
          print("REQUEST FATAL ERROR: "..code);
        else
          set_timeout(function() fatal_callback(code); end, REQUEST_ERROR_DELAY);
        end
      else
        print("SERVER RESPONDED: "..data);
        if success_callback ~= nil then
          success_callback(sjson.decode(data, { null = "NULL" }));        
        end
      end
    end
  );
end

function send_get(url, params, headers, success_callback, repeat_on_error)
  fatal_callback = function() 
    print("repeating GET");
    send_get(url, params, headers, success_callback, repeat_on_error);    
  end;
  if not repeat_on_error then fatal_callback = nil end;

  local uri = build_uri(url, params);
  send_request(uri, "GET", headers, nil, success_callback, fatal_callback);
end

function send_post(url, headers, body, success_callback, repeat_on_error)
  fatal_callback = function() 
    print("repeating POST");
    send_post(url, headers, body, success_callback, repeat_on_error);
  end;
  if not repeat_on_error then fatal_callback = nil end

  if headers ~= nil then 
    headers = headers..APP_JSON_CONTENT_TYPE
  else
    headers = APP_JSON_CONTENT_TYPE
  end

  send_request(url, "POST", headers, body, success_callback, fatal_callback);
end

--------------------------------------------------

function get_pin_state(pin)
  set_pin_state(pin, false) --clear trash signal
  gpio.mode(pin, gpio.INPUT)
  
  if gpio.read(pin) == gpio.HIGH then 
    return true 
  end
  
  return false
end

function set_pin_state(pin, state)
  gpio.mode(pin, gpio.OUTPUT)

  if state then 
    gpio.write(pin, gpio.HIGH) 
  else 
    gpio.write(pin, gpio.LOW) 
  end
end

----------------------------------------------

function send_data() 
  temperature = ds.read(addrs[1], ds.C)

  if not temperature then
    return
  end

  print(temperature)
  
  value_base64 = encoder.toBase64(temperature)

  dto = {
    id = DETECTOR_ID,
    data = {{
        id = DATA_PREFAB_ID,
        data_base64 = value_base64
    }}
  }
  
  data_sending = sjson.encode(dto)
  print(data_sending)
  
  send_post(SERVER_URL, nil, data_sending, function() print("send success") end, true)
end

------------------------------------

print("start")

ds = require('ds18b20')
ds.setup(3)

addrs = ds.addrs()
node_id = node.chipid()
print("Total Sensors.: "..table.getn(addrs).." ")
print("Sensor Type...: "..node_id.." ")
hex_format="%02X%02X%02X%02X%02X%02X%02X%02X"
sensor_count=table.getn(addrs)

for i = 1, sensor_count do
  sid = string.format(hex_format, string.byte(addrs[i], 1, 9))
  print("ds"..i.." Unique ID  : "..sid.." ")
  tmr.wdclr()
end

wifi.setmode(wifi.SOFTAP)
wifi.ap.config(AP_CONFIG)
wifi.ap.setip(IP_CONFIG)

print("access point active")

server = net.createServer(net.TCP)

print("server set up")

local function ap_conncted_callback(e)
  print("started sending data")
  SEND_DATA_TIMER = set_interval(function() send_data() end, SEND_DATA_INTERVAL)
end

local function creds_recieved_callback(ssid, pwd)
  connect_ap(ssid, pwd, ap_conncted_callback)
end

local function creds_recieveing_callback(conn, data)
  creds_reciever(conn, data, creds_recieved_callback) 
end

server:listen(
  PORT_LISTENING, 
  function(conn)
    conn:on("receive", creds_recieveing_callback)
  end
)

print("server is listening to port "..PORT_LISTENING)
