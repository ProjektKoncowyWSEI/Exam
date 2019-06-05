/// <binding AfterBuild='a_copy:libs, b_bundle_js, b_sass, c_bundle_css, d_compress_js, e_compress_css' />

const gulp = require('gulp');
const npmDist = require('gulp-npm-dist');
const rename = require('gulp-rename');
const concat = require('gulp-concat');
const minify = require('gulp-minify');
const cssmin = require('gulp-cssmin');
const concatCss = require('gulp-concat-css');
const sass = require('gulp-sass');
sass.compiler = require('node-sass');

// kopiowanie bibliotek
gulp.task('a_copy:libs', function () {
    gulp.src(npmDist(), { base: './node_modules/' })
        .pipe(rename(function (path) {
            path.dirname = path.dirname.replace(/\/dist/, '').replace(/\\dist/, '');
        }))
        .pipe(gulp.dest('./wwwroot/libs'));
});

//bundle    
//js
gulp.task('b_bundle_js', function () {
    gulp.src(['./wwwroot/js/*.js'])
        .pipe(concat('site.js'))
        .pipe(gulp.dest('./wwwroot/js/bundle'))
});
//css


gulp.task('b_sass', function () {
    return gulp.src('./wwwroot/css/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot/css/'));
});

//gulp.task('b_sass:watch', function () {
//    gulp.watch('./wwwroot/css/*.scss', ['sass']);
//});

gulp.task('c_bundle_css', function () {
    return gulp.src('./wwwroot/css/*.css')
        .pipe(concatCss("site.css"))
        .pipe(gulp.dest('./wwwroot/css/bundle'));
});

//minimalizowanie      

gulp.task('d_compress_js', function () {
    gulp.src(['./wwwroot/js/bundle/*.js'])
        .pipe(minify({
            ext: {
                src: '.org.js',
                min: '.min.js'
            },
            ignoreFiles: ['.min.js']
        }))
        .pipe(gulp.dest('./wwwroot/js/min'))
});

gulp.task('e_compress_css', function () {
    gulp.src('./wwwroot/css/bundle/*.css')
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest('./wwwroot/css/min'));
});