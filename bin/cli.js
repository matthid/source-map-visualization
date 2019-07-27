#!/usr/bin/env node
let path = require("path");
let app = require("../dist/Server/app.js");
let clientDir = path.resolve(__dirname, "../dist/Client");
app.startServerWithArgs(clientDir, process.argv);