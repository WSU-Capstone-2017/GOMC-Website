// Prototype-1-2- gulp file
// Automate tasks for P-1-2

// Variables
var gulp = require('gulp')
var less = require('gulp-less');
var watch = require('gulp-watch');

// Tasks

// Minimize css
gulp.task('minimizeCSS', function(){

});

// change less -> CSS
gulp.task('lessToCss', function() {
  return gulp.src('styles/*.less')
    .pipe(watch('styles/*.less'))
    .pipe(less())
    .pipe(gulp.dest('styles/updated.css'));
});

// Concatenate Javascript
gulp.task('concat',function(){

});

// Lint Javascript
gulp.task('lint', function(){

});

//  Run main automation suite
gulp.task('default', function(){

});
