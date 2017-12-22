var util = require('gulp-util');
var notify = require('gulp-notify');

module.exports = function(err) {

   var name = util.colors.red(err.name + ':');
   var message = err.message;
   var output = util.colors.yellow(message.replace(/\n|\r/g, ''));

   util.log(name);
   util.log(output);

   /*
   return notify.onError({
      title: 'Gulp',
      message: 'Error: <%= message %>',
      sound: 'Bottle'
   });
   */
   
}