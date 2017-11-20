/// <binding ProjectOpened='default' />

var gulp = require('gulp');
var sass = require('gulp-sass');

gulp.task('default', function() {
  console.log("Running gulp");

    return gulp.src('Content/Styles/styles.scss')
        .pipe(sass())
        .pipe(gulp.dest('wwwroot/Dist'));
});
