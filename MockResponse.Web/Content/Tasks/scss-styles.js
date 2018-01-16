var del = require('del');
var compile = require('./utilities/scss-compile');

module.exports = function (config, gulp) {

   return function () {

      var src = config.path.styles + config.src.styles;

      return del([
         config.path.dist + 'style*.css*'
      ]).then(function() {
         return compile(gulp, src, config.asset.style, false);

      });

   };

};
