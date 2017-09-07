// Load the http module to create an http server.
var http = require('http');
var dns = require('dns');

var server = http.createServer(function (request, response) {
	var nodeName = process.env.Fabric_NodeName;

	dns.resolve("pythonbackend.simplecontainerapp",'SRV', function(err, addresses){ 
		if (err){
			response.end(err.code);
		} else {
			var host = addresses[0].name;
			var port = addresses[0].port;

			var options = {
				host: 'pythonbackend.simplecontainerapp',
				port: addresses[0].port
			  };
			  
			callback = function(res) {
				var str = 'Python backend is running on: ';
				
				//another chunk of data has been recieved, so append it to `str`
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
			
			http.request(options, callback).end();
		}
	});

});

// Listen on port 8000, IP defaults to 127.0.0.1
server.listen(8000);