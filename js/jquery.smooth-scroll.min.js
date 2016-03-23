/*!
 * jQuery Smooth Scroll Plugin v1.4.4
 *
 * Date: Mon Feb 20 09:04:54 2012 EST
 * Requires: jQuery v1.3+
 *
 * Copyright 2010, Karl Swedberg
 * Dual licensed under the MIT and GPL licenses (just like jQuery):
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 *
 *
 *
*/
(function(b){function m(c){return c.replace(/(:|\.)/g,"\\$1")}var n=function(c){var e=[],a=false,d=c.dir&&c.dir=="left"?"scrollLeft":"scrollTop";this.each(function(){if(!(this==document||this==window)){var f=b(this);if(f[d]()>0)e.push(this);else{f[d](1);a=f[d]()>0;f[d](0);a&&e.push(this)}}});if(c.el==="first"&&e.length)e=[e.shift()];return e},o="ontouchend"in document;b.fn.extend({scrollable:function(c){return this.pushStack(n.call(this,{dir:c}))},firstScrollable:function(c){return this.pushStack(n.call(this,
{el:"first",dir:c}))},smoothScroll:function(c){c=c||{};var e=b.extend({},b.fn.smoothScroll.defaults,c),a=b.smoothScroll.filterPath(location.pathname);this.die("click.smoothscroll").live("click.smoothscroll",function(d){var f={},j=b(this),g=location.hostname===this.hostname||!this.hostname,h=e.scrollTarget||(b.smoothScroll.filterPath(this.pathname)||a)===a,k=m(this.hash),i=true;if(!e.scrollTarget&&(!g||!h||!k))i=false;else{g=e.exclude;h=0;for(var l=g.length;i&&h<l;)if(j.is(m(g[h++])))i=false;g=e.excludeWithin;
h=0;for(l=g.length;i&&h<l;)if(j.closest(g[h++]).length)i=false}if(i){d.preventDefault();b.extend(f,e,{scrollTarget:e.scrollTarget||k,link:this});b.smoothScroll(f)}});return this}});b.smoothScroll=function(c,e){var a,d,f,j=0;d="offset";var g="scrollTop",h={},k=false;f=[];if(typeof c==="number"){a=b.fn.smoothScroll.defaults;f=c}else{a=b.extend({},b.fn.smoothScroll.defaults,c||{});if(a.scrollElement){d="position";a.scrollElement.css("position")=="static"&&a.scrollElement.css("position","relative")}f=
e||b(a.scrollTarget)[d]()&&b(a.scrollTarget)[d]()[a.direction]||0}a=b.extend({link:null},a);g=a.direction=="left"?"scrollLeft":g;if(a.scrollElement){d=a.scrollElement;j=d[g]()}else{d=b("html, body").firstScrollable();k=o&&"scrollTo"in window}h[g]=f+j+a.offset;a.beforeScroll.call(d,a);if(k){f=a.direction=="left"?[h[g],0]:[0,h[g]];window.scrollTo.apply(window,f);a.afterScroll.call(a.link,a)}else d.animate(h,{duration:a.speed,easing:a.easing,complete:function(){a.afterScroll.call(a.link,a)}})};b.smoothScroll.version=
"1.4.4";b.smoothScroll.filterPath=function(c){return c.replace(/^\//,"").replace(/(index|default).[a-zA-Z]{3,4}$/,"").replace(/\/$/,"")};b.fn.smoothScroll.defaults={exclude:[],excludeWithin:[],offset:0,direction:"top",scrollElement:null,scrollTarget:null,beforeScroll:function(){},afterScroll:function(){},easing:"swing",speed:400}})(jQuery);