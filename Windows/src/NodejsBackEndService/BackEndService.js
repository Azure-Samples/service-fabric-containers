// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

// Get a list of the local IP addresses and display them. 
// This code is needed for a container to work around the Windows Server NAT issue.
'use strict';
var os = require('os');
var ifaces = os.networkInterfaces();
var result;
Object.keys(ifaces).forEach(function (ifname) {
    var alias = 0;
    ifaces[ifname].forEach(function (iface) {
        if ('IPv4' !== iface.family || iface.internal !== false) {
            return;
        }
        console.log(ifname, iface.address);
        result = iface.address;
        ++alias;
    });
});

// Start server and load index.html getting the environment Service Fabric environment variable for the node name
console.log("Service Fabric Nodejs container service");
console.log("Fabric Node: %s", process.env.Fabric_NodeName);
var http = require('http');
var fs = require('fs');
fs.readFile('./jslocation/index.html', function (err, html) {
    if (err) {
        throw err;
    }
    console.log("Starting web server. Listening on port 8905");
    if (process.env.IsContainer != "true")
    {
        result = process.env.Fabric_NodeIPOrFQDN;
    }
    console.log("Listening on address " + result);
    http.createServer(function (req, res) {
        res.writeHead(200, { 'Content-Type': 'text/html' });
        res.write("<!DOCTYPE html>");
        res.write("<html>");
        res.write("<body>");
        res.write("<h1>");
        res.write("Node Name: " + process.env.Fabric_NodeName);
        res.write("</h1>");
        res.write("</body>");
        res.write("</html>");
        res.write(html);
        res.end();
    }).listen(8905, result);
});