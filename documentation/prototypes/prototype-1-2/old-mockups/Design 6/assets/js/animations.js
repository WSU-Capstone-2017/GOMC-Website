/*
 * jQuery appear plugin
 *
 * Copyright (c) 2012 Andrey Sidorov
 * licensed under MIT license.
 *
 * https://github.com/morr/jquery.appear/
 *
 * Version: 0.3.3
 */
(function(e){function u(){r=false;for(var n=0;n<t.length;n++){var i=e(t[n]).filter(function(){return e(this).is(":appeared")});i.trigger("appear",[i]);if(o){var s=o.not(i);s.trigger("disappear",[s])}o=i}}var t=[];var n=false;var r=false;var i={interval:250,force_process:false};var s=e(window);var o;e.expr[":"]["appeared"]=function(t){var n=e(t);if(!n.is(":visible")){return false}var r=s.scrollLeft();var i=s.scrollTop();var o=n.offset();var u=o.left;var a=o.top;if(a+n.height()>=i&&a+(n.data("appear-bottom-offset")||0)<=i+s.height()&&u+n.width()>=r&&u-(n.data("appear-left-offset")||0)<=r+s.width()){return true}else{return false}};e.fn.extend({appear:function(s){var o=e.extend({},i,s||{});var a=this.selector||this;if(!n){var f=function(){if(r){return}r=true;setTimeout(u,o.interval)};e(window).scroll(f).resize(f);n=true}if(o.force_process){setTimeout(u,o.interval)}t.push(a);return e(a)}});e.extend({force_appear:function(){if(n){u();return true}return false}})})(jQuery)


//----ANIMATIONS---//
jQuery(document).ready(function($){

jQuery('.animated').appear();

    jQuery(document.body).on('appear', '.fade', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('anim-fade') });
    });
    jQuery(document.body).on('appear', '.slidea', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('anim-slide') });
    });
    jQuery(document.body).on('appear', '.hatch', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('anim-hatch') });
    });
    jQuery(document.body).on('appear', '.entrance', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('anim-entrance') });
    });
	jQuery(document.body).on('appear', '.fadeInUpNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('fadeInUp') });
    });
	jQuery(document.body).on('appear', '.fadeInDownNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('fadeInDown') });
    });
	jQuery(document.body).on('appear', '.fadeInLeftNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('fadeInLeft') });
    });
	jQuery(document.body).on('appear', '.fadeInRightNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('fadeInRight') });
    });
	
	
	jQuery(document.body).on('appear', '.fadeInUpBigNow', function() {
    jQuery(this).each(function(){ jQuery(this).addClass('fadeInUpBig') });
    });
	jQuery(document.body).on('appear', '.fadeInDownBigNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('fadeInDownBig') });
    });
	jQuery(document.body).on('appear', '.fadeInLeftBigNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('fadeInLeftBig') });
    });
	jQuery(document.body).on('appear', '.fadeInRightBigNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('fadeInRightBig') });
    });
	
	jQuery(document.body).on('appear', '.fadeInNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('fadeIn') });
    });
	jQuery(document.body).on('appear', '.flashNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('flash') });
    });
	jQuery(document.body).on('appear', '.shakeNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('shake') });
    });
	jQuery(document.body).on('appear', '.bounceNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('bounce') });
    });
	jQuery(document.body).on('appear', '.tadaNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('tada') });
    });
	jQuery(document.body).on('appear', '.swingNow', function() {
        jQuery(this).each(function(){ jQuery(this).addClass('swing') });
    });
});