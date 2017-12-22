var when = require('gulp-if');
var tap = require('gulp-tap');
var sass = require('gulp-sass');
var hash = require('gulp-hash');
var rename = require('gulp-rename');
var autoprefix = require('gulp-autoprefixer');
var sourcemaps = require('gulp-sourcemaps');
var config = require('../config');
var error = require('../error');


/* -----------------------------------
 *
 * Release
 *
 * -------------------------------- */

var release = process.argv.includes('--release');


/* -----------------------------------
 *
 *  SCSS Compile
 *
 * -------------------------------- */

module.exports = function (gulp, src, name, change) {

   var stream = gulp
   .src(src)
   .pipe(
      when(!release, 
         sourcemaps.init()
      )
   )
   .pipe(
      sass({
         outputStyle: 'compressed',
         includePaths: [
            'node_modules/susy/sass'
         ]
      })  
   )
   .pipe(
      autoprefix(
         config.autoprefix
      )
   );

   if (change) {
      stream.
      pipe(
         rename(name)
      );
   }

   stream
   .pipe(
      hash(release ? config.hash.enable : config.hash.disable)
   )
   .pipe(
      when(!release, 
         sourcemaps.write('./')
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

   return stream;

}
