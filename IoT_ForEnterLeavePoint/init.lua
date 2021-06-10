AP_CONFIG = { ssid = "esp8266_enter_leave_point", pwd = "11111111", auth = wifi.WPA2_PSK, max = 1 }
IP_CONFIG = { ip = "192.168.1.1", netmask = "255.255.255.252", gateway = "192.168.1.1" }
PORT_LISTENING = 80

SERVER_URL = "http://workautomatorback.azurewebsites.net/api/"
APP_JSON_CONTENT_TYPE = 'Content-Type: application/json\r\n'

ADD_ENTER_EVENT_DATA_ENDPOINT = "Location/TryEnter"
ADD_LEAVE_EVENT_DATA_ENDPOINT = "Location/TryLeave"

REQUEST_ERROR_DELAY = 1000

USER_ID = 2
ENTER_LEAVE_POINT_ID = 2

CHECK_INTERVAL = 500
RECOVERY_INTERVAL = 1000

RESULT_YES_PIN = 4
RESULT_NO_PIN = 3

ENTER_INFO = { 
  name = "enter", check_pin = 1, 
  endpoint = ADD_ENTER_EVENT_DATA_ENDPOINT, 
  check_timer = nil, recovery_timer = nil 
}
LEAVE_INFO = { 
  name = "leave", check_pin = 2, 
  endpoint = ADD_LEAVE_EVENT_DATA_ENDPOINT, 
  check_timer = nil, recovery_timer = nil 
}

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

function init_check_event_loop(info)
  print("checking started for "..info.name)
  info.check_timer = set_interval(function() check_event(info) end, CHECK_INTERVAL)
end

function check_event(info) 
  print("check iteration for "..info.name)
  if get_pin_state(info.check_pin) then
    print(info.name.." was used")
    info.check_timer:unregister();

    dto = {
	  enter_leave_point_id = ENTER_LEAVE_POINT_ID,
      account_id = USER_ID
    }
        
    send_post(
      SERVER_URL..info.endpoint, nil, sjson.encode(dto),
      function(data) check_event_callback(info, data) end, true
    )
  end
end

function check_event_callback(info, data)
  if data.error == nil or data.error == sjson.NULL or data.error == "NULL" then
    print(info.name.." was successful")
    set_pin_state(RESULT_YES_PIN, true);
  else
    print(info.name.." failed")
    set_pin_state(RESULT_NO_PIN, true);
  end

  print("recovery started for "..info.name)
  info.recovery_timer = set_interval(function() check_event_recovery(info) end, RECOVERY_INTERVAL)
end

function check_event_recovery(info)
  print("recovery iteration for "..info.name)
  if not get_pin_state(info.check_pin) then
    print("recovery finished for "..info.name)
    
    info.recovery_timer:unregister()
    info.check_timer = set_interval(function() check_event(info) end, CHECK_INTERVAL)

    set_pin_state(RESULT_YES_PIN, false);
    set_pin_state(RESULT_NO_PIN, false);
  end
end

------------------------------------

print("start")

set_pin_state(RESULT_YES_PIN, false);
set_pin_state(RESULT_NO_PIN, false);

wifi.setmode(wifi.SOFTAP)
wifi.ap.config(AP_CONFIG)
wifi.ap.setip(IP_CONFIG)

print("access point active")

server = net.createServer(net.TCP)

print("server set up")

local function ap_conncted_callback(e)
  init_check_event_loop(ENTER_INFO)
  init_check_event_loop(LEAVE_INFO)
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
