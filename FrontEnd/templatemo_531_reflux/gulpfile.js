var gulp = require('gulp');
var concat = require('gulp-concat');
var cleanCSS = require('gulp-clean-css');
var minify = require('gulp-minify');

const cssFiles = [
	'vendor/**/*.css',
	'assets/css/fontawesome.css',
	'assets/css/owl.css',
	'assets/css/templatemo-style.css',
	'assets/css/custom.css'
];
// create task
gulp.task('css', function(){
    return gulp.src(cssFiles)
        .pipe(cleanCSS({compatibility: 'ie8'}))
        .pipe(concat('style.min.css'))
        .pipe(gulp.dest('dist/css'))
});

gulp.task('js', function(){
    gulp.src(['assets/js/*.js'])
        .pipe(minify({noSource:true}))
        .pipe(concat('site.min.js'))
        .pipe(gulp.dest('dist/js'));
    return gulp.src(['vendor/**/*.js'])
        .pipe(minify({noSource:true}))
        .pipe(concat('vendor.min.js'))
        .pipe(gulp.dest('dist/js'));
});

gulp.task('assets', function(){
	gulp.src(['assets/images/*']).pipe(gulp.dest('dist/images'));
	return gulp.src(['assets/fonts/*']).pipe(gulp.dest('dist/fonts'));
});

gulp.task('default', gulp.series('css', 'js', 'assets'));