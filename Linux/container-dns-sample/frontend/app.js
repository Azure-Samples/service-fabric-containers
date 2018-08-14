// Load the http module to create an http server.
var http = require('http');
var dns = require('dns');

var server = http.createServer(function (request, response) {
	var nodeName = process.env.Fabric_NodeName;
	var ipAddress = '';
	var port = 80;	

	dns.resolve("pythonbackend.simplecontainerapp", function(errors, ipAddresses){
		if (errors){
			response.end(errors.message);
		} else {

			//extract ip address of an instance of the backend service
			ipAddress = ipAddresses[0];

			//build options JSON for the http request
			var options = {
				host: ipAddress,
				port: port
			  };
			
			//define the callback of the backend http request
			callback = function(res) {
				var str = 'Python backend is running on: ';
				
				//chunk of data received, append to str
				res.on('data', function (chunk) {
					str += chunk;
				});
				
				//the whole response has been recieved, so we just print it out here
				res.on('end', function () {
					str += " \nNodeJS frontend is running on: ";
					str += nodeName;
					response.end(str);
				});
			}
			
			//make the http request to the backend 
			var req = http.request(options, callback);

			//error received while making http request to backend
			req.on("error", (err) => {
				response.end(err.message);		
			});

			req.end();
		}
	});

	//error received so display error
	request.on('error', (err)=>{
		response.end(err.message);
	});

});

// Listen on port 8000, IP defaults to 127.0.0.1
server.listen(8000);
