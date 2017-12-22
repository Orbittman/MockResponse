var browserify = require('browserify');
var uglify = require('gulp-uglify');
var when = require('gulp-if');
var tap = require('gulp-tap');
var buffer = require('vinyl-buffer');
var source = require('vinyl-source-stream');
var del = require('del');
var hash = require('gulp-hash');
var error = require('./error');
var manifest = require('../../package.json');
var dependencies = Object.keys(manifest && manifest.appDependencies || {});


/* -----------------------------------
 *
 * Flags
 *
 * -------------------------------- */

var release = process.argv.includes('--release');


/* -----------------------------------
 *
 * Vendor
 *
 * -------------------------------- */

module.exports = function (config, gulp) {

   if (release) {
      process.env.NODE_ENV = 'production';
   }

   return function () {

      return del([
         config.path.dist + 'vendor*.js'
      ]).then(function () {

         return browserify()
            .require(dependencies)
            .bundle()
            .on('error', error)
            .pipe(
               source(config.asset.vendor)
            )
            .pipe(
               buffer()
            )
            .pipe(
               uglify(config.uglify)
            )
            .pipe(
               hash(
                  release ? config.hash.enable : config.hash.disable
               )
            )
            .pipe(
               gulp.dest(config.path.dist)
            )
            .pipe(
               hash.manifest(config.asset.manifest, {
                  deleteOld: false,
                  sourceDir: config.path.dist
               })
            )
            .pipe(
               gulp.dest(config.path.dist)
            );
            
      });

   };

};
