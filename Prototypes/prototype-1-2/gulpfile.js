// Prototype-1-2- gulp file
// Automate tasks for P-1-2
// Variables


// imported file
var gulp = require('gulp')
var gulp = require('gulp');
var less = require('gulp-less');
var watch = require('gulp-watch');


gulp.task('lessToCss', function() {
  return gulp.src('styles/*.less')
    .pipe(watch('styles/*.less'))
    .pipe(less())
    .pipe(gulp.dest('styles/updated.css'));
});

// end import


var gulp = require('gulp')

// Concatenate Javascript
gulp.task('concat',function(){

});

// Minimize css
gulp.task('minimizeCSS', function(){

});

// change less -> CSS
gulp.task('lessToCSS', function(){

});

// Lint Javascript
gulp.task('lint', function(){

});

//  Run main automation suite
gulp.task('default', function(){

});
