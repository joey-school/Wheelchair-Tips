var gulp = require('gulp');
var sass = require('gulp-sass');

gulp.task('styles', function() {
    gulp.src('wwwroot/css/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot/css'))
});

//Watch task
gulp.task('default',function() {
    gulp.watch('wwwroot/css/*.scss', ['styles']);
});