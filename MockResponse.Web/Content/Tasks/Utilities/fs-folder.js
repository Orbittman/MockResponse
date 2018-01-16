var fs = require('fs');
var path = require('path');
var config = require('../config');

module.exports = function(dir) {

   return fs
   .readdirSync(dir)
   .filter(function (file) {
      return fs.statSync(path.join(dir, file)).isDirectory();
   });

}