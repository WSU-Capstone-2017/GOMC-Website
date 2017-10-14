// Prototype-1-2- gulp file
// Automate tasks for P-1-2

// Variables
var gulp = require('gulp')
var less = require('gulp-less');
var watch = require('gulp-watch');

// Tasks

// validate less
gulp.task('lessValid', function(){

});

// validate html
gulp.task('lessValid', function(){
    
});

// validate minimize html
gulp.task('lessValid', function(){

});


// change less -> CSS
gulp.task('lessToCss', function() {
  return gulp.src('styles/*.less')
    .pipe(watch('styles/*.less'))
    .pipe(less())
    .pipe(gulp.dest('styles/updated.css'));
});

// Minimize css
gulp.task('minimizeCSS', function(){

});

// Lint Javascript
gulp.task('lint', function(){

});

// Concatenate Javascript
gulp.task('concat',function(){

});



//  Run main automation suite
gulp.task('default', function(){

});
