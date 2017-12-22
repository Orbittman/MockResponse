/// <binding ProjectOpened='default' />

var gulp = require('gulp');
var sequence = require('gulp-sequence');
var util = require('gulp-util');
var server = require('browser-sync').create();

var config = require('./Content/tasks/config');

/* Task setup */
function task(task, attr = null) {
    return require('./Content/tasks/' + task)(config, gulp, attr);
}


/* Client JS */
gulp.task('js:client', task('js-client'));

/* Vendor JS */
gulp.task('js:vendor', task('js-vendor'));

/* Styles */
gulp.task('scss:styles', task('scss-styles'));

/* default task */
gulp.task('default', sequence('js:client', 'scss:styles', 'js:vendor'));
