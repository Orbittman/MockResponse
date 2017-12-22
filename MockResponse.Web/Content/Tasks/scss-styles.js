var browserify = require('browserify');
var hash = require('gulp-hash');
var util = require('gulp-util');
var del = require('del');
var path = require('path');
var error = require('./error');
var getFolders = require('./utilities/fs-folder');
var compile = require('./utilities/scss-compile');
var merge = require('merge-stream');
var rename = require('gulp-rename');


/* -----------------------------------
 *
 * Style
 *
 * -------------------------------- */

module.exports = function (config, gulp) {

   return function () {

      var src = config.styles.path + config.styles.file;

      return del([
         config.path.dist + 'style*.css*'
      ]).then(function() {
         return compile(gulp, src, config.asset.style, false);

      });

   };

};
