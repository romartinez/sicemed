/*
 * jQuery Tools 1.2.5 - The missing UI library for the Web
 * 
 * [tabs, tabs.slideshow]
 * 
 * NO COPYRIGHTS OR LICENSES. DO WHAT YOU LIKE.
 * 
 * http://flowplayer.org/tools/
 * 
 * File generated: Fri Oct 22 08:53:19 GMT 2010
 */
(function(c){function p(d,b,a){var e=this,l=d.add(this),h=d.find(a.tabs),i=b.jquery?b:d.children(b),j;h.length||(h=d.children());i.length||(i=d.parent().find(b));i.length||(i=c(b));c.extend(this,{click:function(f,g){var k=h.eq(f);if(typeof f=="string"&&f.replace("#","")){k=h.filter("[href*="+f.replace("#","")+"]");f=Math.max(h.index(k),0)}if(a.rotate){var n=h.length-1;if(f<0)return e.click(n,g);if(f>n)return e.click(0,g)}if(!k.length){if(j>=0)return e;f=a.initialIndex;k=h.eq(f)}if(f===j)return e;
g=g||c.Event();g.type="onBeforeClick";l.trigger(g,[f]);if(!g.isDefaultPrevented()){o[a.effect].call(e,f,function(){g.type="onClick";l.trigger(g,[f])});j=f;h.removeClass(a.current);k.addClass(a.current);return e}},getConf:function(){return a},getTabs:function(){return h},getPanes:function(){return i},getCurrentPane:function(){return i.eq(j)},getCurrentTab:function(){return h.eq(j)},getIndex:function(){return j},next:function(){return e.click(j+1)},prev:function(){return e.click(j-1)},destroy:function(){h.unbind(a.event).removeClass(a.current);
i.find("a[href^=#]").unbind("click.T");return e}});c.each("onBeforeClick,onClick".split(","),function(f,g){c.isFunction(a[g])&&c(e).bind(g,a[g]);e[g]=function(k){k&&c(e).bind(g,k);return e}});if(a.history&&c.fn.history){c.tools.history.init(h);a.event="history"}h.each(function(f){c(this).bind(a.event,function(g){e.click(f,g);return g.preventDefault()})});i.find("a[href^=#]").bind("click.T",function(f){e.click(c(this).attr("href"),f)});if(location.hash&&a.tabs=="a"&&d.find("[href="+location.hash+"]").length)e.click(location.hash);
else if(a.initialIndex===0||a.initialIndex>0)e.click(a.initialIndex)}c.tools=c.tools||{version:"1.2.5"};c.tools.tabs={conf:{tabs:"a",current:"current",onBeforeClick:null,onClick:null,effect:"default",initialIndex:0,event:"click",rotate:false,history:false},addEffect:function(d,b){o[d]=b}};var o={"default":function(d,b){this.getPanes().hide().eq(d).show();b.call()},fade:function(d,b){var a=this.getConf(),e=a.fadeOutSpeed,l=this.getPanes();e?l.fadeOut(e):l.hide();l.eq(d).fadeIn(a.fadeInSpeed,b)},slide:function(d,
b){this.getPanes().slideUp(200);this.getPanes().eq(d).slideDown(400,b)},ajax:function(d,b){this.getPanes().eq(0).load(this.getTabs().eq(d).attr("href"),b)}},m;c.tools.tabs.addEffect("horizontal",function(d,b){m||(m=this.getPanes().eq(0).width());this.getCurrentPane().animate({width:0},function(){c(this).hide()});this.getPanes().eq(d).animate({width:m},function(){c(this).show();b.call()})});c.fn.tabs=function(d,b){var a=this.data("tabs");if(a){a.destroy();this.removeData("tabs")}if(c.isFunction(b))b=
{onBeforeClick:b};b=c.extend({},c.tools.tabs.conf,b);this.each(function(){a=new p(c(this),d,b);c(this).data("tabs",a)});return b.api?a:this}})(jQuery);
(function(c){function p(g,a){function m(f){var e=c(f);return e.length<2?e:g.parent().find(f)}var b=this,i=g.add(this),d=g.data("tabs"),h,j=true,n=m(a.next).click(function(){d.next()}),k=m(a.prev).click(function(){d.prev()});c.extend(b,{getTabs:function(){return d},getConf:function(){return a},play:function(){if(h)return b;var f=c.Event("onBeforePlay");i.trigger(f);if(f.isDefaultPrevented())return b;h=setInterval(d.next,a.interval);j=false;i.trigger("onPlay");return b},pause:function(){if(!h)return b;
var f=c.Event("onBeforePause");i.trigger(f);if(f.isDefaultPrevented())return b;h=clearInterval(h);i.trigger("onPause");return b},stop:function(){b.pause();j=true}});c.each("onBeforePlay,onPlay,onBeforePause,onPause".split(","),function(f,e){c.isFunction(a[e])&&c(b).bind(e,a[e]);b[e]=function(q){return c(b).bind(e,q)}});a.autopause&&d.getTabs().add(n).add(k).add(d.getPanes()).hover(b.pause,function(){j||b.play()});a.autoplay&&b.play();a.clickable&&d.getPanes().click(function(){d.next()});if(!d.getConf().rotate){var l=
a.disabledClass;d.getIndex()||k.addClass(l);d.onBeforeClick(function(f,e){k.toggleClass(l,!e);n.toggleClass(l,e==d.getTabs().length-1)})}}var o;o=c.tools.tabs.slideshow={conf:{next:".forward",prev:".backward",disabledClass:"disabled",autoplay:false,autopause:true,interval:3E3,clickable:true,api:false}};c.fn.slideshow=function(g){var a=this.data("slideshow");if(a)return a;g=c.extend({},o.conf,g);this.each(function(){a=new p(c(this),g);c(this).data("slideshow",a)});return g.api?a:this}})(jQuery);
