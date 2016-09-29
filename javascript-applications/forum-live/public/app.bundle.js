/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};

/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {

/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId])
/******/ 			return installedModules[moduleId].exports;

/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			exports: {},
/******/ 			id: moduleId,
/******/ 			loaded: false
/******/ 		};

/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);

/******/ 		// Flag the module as loaded
/******/ 		module.loaded = true;

/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}


/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;

/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;

/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";

/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ function(module, exports, __webpack_require__) {

	const Router = __webpack_require__(1);

	const RequestProvider = __webpack_require__(5);
	const DataServise = __webpack_require__(6);
	const TemplateLoaderService = __webpack_require__(4);

	// controllers
	const LoginController = __webpack_require__(3);
	const HomeController = __webpack_require__(7);

	(() => {
	    const URLS = {
	        USERS: 'api/users'
	    };

	    const requests = new RequestProvider();

	    const data = new DataServise(URLS, requests);
	    const templates = new TemplateLoaderService(requests);

	    const home = new HomeController();
	    const login = new LoginController(templates, data);

	    const my = new Router('#content', home, login);
	    my.start();

	})();

/***/ },
/* 1 */
/***/ function(module, exports, __webpack_require__) {

	const Navigo = __webpack_require__(2);

	const Router = (() => {
	    class Router {
	        constructor(contentContainerId, homeController, loginController) {
	            this.contentId = contentContainerId;

	            this.homeController = homeController;
	            this.loginController = loginController;

	            this.__initRouter__();
	        }

	        start() {
	            this._router.resolve();
	        }

	        __initRouter__() {
	            const that = this;
	            that._router = new Navigo(null, true);

	            that._router.on(() => {
	                that._router.navigate('/');
	            });

	            that._router.on('/login', () => {
	                that.loginController.start(that.contentId);
	            });

	            that._router.on('/', () => {
	                that.homeController.start(that.contentId);
	            });
	        }
	    }

	    return Router;
	})();

	module.exports = Router;

/***/ },
/* 2 */
/***/ function(module, exports, __webpack_require__) {

	!function(t,e){ true?module.exports=e():"function"==typeof define&&define.amd?define("Navigo",[],e):"object"==typeof exports?exports.Navigo=e():t.Navigo=e()}(this,function(){return function(t){function e(r){if(n[r])return n[r].exports;var o=n[r]={exports:{},id:r,loaded:!1};return t[r].call(o.exports,o,o.exports,e),o.loaded=!0,o.exports}var n={};return e.m=t,e.c=n,e.p="",e(0)}([function(t,e){"use strict";function n(t){if(Array.isArray(t)){for(var e=0,n=Array(t.length);e<t.length;e++)n[e]=t[e];return n}return Array.from(t)}function r(t){return t instanceof RegExp?t:t.replace(/\/+$/,"").replace(/^\/+/,"/")}function o(t,e){return 0===e.length?null:t?t.slice(1,t.length).reduce(function(t,n,r){return null===t&&(t={}),t[e[r]]=n,t},null):null}function i(t){var e,n=[];return e=t instanceof RegExp?t:new RegExp(r(t).replace(_,function(t,e,r){return n.push(r),v}).replace(g,m)+y),{regexp:e,paramNames:n}}function u(t){return t.replace(/\/$/,"").split("/").length}function s(t,e){return u(t)<u(e)}function a(t){var e=arguments.length<=1||void 0===arguments[1]?[]:arguments[1];return e.map(function(e){var n=i(e.route),r=n.regexp,u=n.paramNames,s=t.match(r),a=o(s,u);return s?{match:s,route:e,params:a}:!1}).filter(function(t){return t})}function l(t,e){return a(t,e)[0]||!1}function c(t,e){var n=a(t,e.filter(function(t){var e=r(t.route);return""!==e&&"*"!==e})),o=r(t);return n.length>0?n.map(function(e){return r(t.substr(0,e.match.index))}).reduce(function(t,e){return e.length<t.length?e:t},o):o}function f(){return!("undefined"==typeof window||!window.history||!window.history.pushState)}function d(t){return t.replace(/\?(.*)?$/,"")}function h(t,e){this._routes=[],this.root=e&&t?t.replace(/\/$/,"/#"):t||null,this._useHash=e,this._paused=!1,this._destroyed=!1,this._lastRouteResolved=null,this._notFoundHandler=null,this._defaultHandler=null,this._ok=!e&&f(),this._listen(),this.updatePageLinks()}var p="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(t){return typeof t}:function(t){return t&&"function"==typeof Symbol&&t.constructor===Symbol?"symbol":typeof t};Object.defineProperty(e,"__esModule",{value:!0});var _=/([:*])(\w+)/g,g=/\*/g,v="([^/]+)",m="(?:.*)",y="(?:/$|$)";h.prototype={helpers:{match:l,root:c,clean:r},navigate:function(t,e){var n;return t=t||"",this._ok?(n=(e?"":this._getRoot()+"/")+r(t),n=n.replace(/([^:])(\/{2,})/g,"$1/"),history[this._paused?"replaceState":"pushState"]({},"",n),this.resolve()):"undefined"!=typeof window&&(window.location.href=window.location.href.replace(/#(.*)$/,"")+"#"+t),this},on:function(){for(var t=this,e=arguments.length,n=Array(e),r=0;e>r;r++)n[r]=arguments[r];if(n.length>=2)this._add(n[0],n[1]);else if("object"===p(n[0])){var o=Object.keys(n[0]).sort(s);o.forEach(function(e){t._add(e,n[0][e])})}else"function"==typeof n[0]&&(this._defaultHandler=n[0]);return this},notFound:function(t){this._notFoundHandler=t},resolve:function(t){var e,r,o=(t||this._cLoc()).replace(this._getRoot(),"");return this._paused||o===this._lastRouteResolved?!1:(this._useHash&&(o=o.replace(/^\/#/,"/")),o=d(o),(r=l(o,this._routes))?(this._lastRouteResolved=o,e=r.route.handler,r.route.route instanceof RegExp?e.apply(void 0,n(r.match.slice(1,r.match.length))):e(r.params),r):!this._defaultHandler||""!==o&&"/"!==o?(this._notFoundHandler&&this._notFoundHandler(),!1):(this._lastRouteResolved=o,this._defaultHandler(),!0))},destroy:function(){this._routes=[],this._destroyed=!0,clearTimeout(this._listenningInterval),"undefined"!=typeof window?window.onpopstate=null:null},updatePageLinks:function(){var t=this;"undefined"!=typeof document&&this._findLinks().forEach(function(e){e.hasListenerAttached||(e.addEventListener("click",function(n){var o=e.getAttribute("href");t._destroyed||(n.preventDefault(),t.navigate(r(o)))}),e.hasListenerAttached=!0)})},generate:function(t){var e=arguments.length<=1||void 0===arguments[1]?{}:arguments[1];return this._routes.reduce(function(n,r){var o;if(r.name===t){n=r.route;for(o in e)n=n.replace(":"+o,e[o])}return n},"")},link:function(t){return this._getRoot()+t},pause:function(t){this._paused=t},disableIfAPINotAvailable:function(){f()||this.destroy()},_add:function(t){var e=arguments.length<=1||void 0===arguments[1]?null:arguments[1];return"object"===("undefined"==typeof e?"undefined":p(e))?this._routes.push({route:t,handler:e.uses,name:e.as}):this._routes.push({route:t,handler:e}),this._add},_getRoot:function(){return null!==this.root?this.root:(this.root=c(this._cLoc(),this._routes),this.root)},_listen:function(){var t=this;this._ok?window.onpopstate=function(){t.resolve()}:!function(){var e=t._cLoc(),n=void 0,r=void 0;(r=function(){n=t._cLoc(),e!==n&&(e=n,t.resolve()),t._listenningInterval=setTimeout(r,200)})()}()},_cLoc:function(){return"undefined"!=typeof window?window.location.href:""},_findLinks:function(){return[].slice.call(document.querySelectorAll("[data-navigo]"))}},e["default"]=h,t.exports=e["default"]}])});
	//# sourceMappingURL=navigo.min.js.map

/***/ },
/* 3 */
/***/ function(module, exports) {

	const LoginController = (() => {
	    class LoginController {
	        constructor(templateService, dataService) {
	            this.templateService = templateService;
	            this.dataService = dataService;
	        }

	        start(containerId) {
	            const that = this;

	            const container = $(containerId);
	            return this.templateService.get('login')
	                .then((template) => {
	                    const html = template();
	                    container.html(html);
	                    return html;
	                })
	                .then((html) => {
	                    const btnLogin = container.find('#btn-login');
	                    btnLogin.on('click', (ev) => {
	                        const username = container.find('#tb-username');
	                        const password = container.find('#tb-password');
	                        const user = {
	                            username: username.val(),
	                            passHash: password.val()
	                        };

	                        that.dataService.userLogin(user)
	                            .then((response) => {
	                                console.log(response);
	                                that._saveUser(response);
	                                return response;
	                            });
	                    });
	                });
	        }

	        _saveUser(user) {
	            window.sessionStorage.setItem('USER_NAME', user.username);
	            window.sessionStorage.setItem('AUTH_KEY', user.authKey);
	        }
	    }

	    return LoginController;
	})();

	module.exports = LoginController;

/***/ },
/* 4 */
/***/ function(module, exports) {

	const TemplateLoaderService = (() => {
	    class TemplateLoaderService {
	        constructor(requestsProvider) {
	            this._requests = requestsProvider;
	        }

	        get(template) {
	            const url = `/templates/${template}.html`;
	            return this._requests.get(url)
	                .then((data) => {
	                    const compiled = Handlebars.compile(data);
	                    return compiled;
	                });
	        }
	    }

	    return TemplateLoaderService;
	})();

	module.exports = TemplateLoaderService;

/***/ },
/* 5 */
/***/ function(module, exports) {

	const RequestProvider = (() => {
	    class RequestsProvider {
	        get(url) {
	            return new Promise((resolve, reject) => {
	                $.ajax({
	                    url: url,
	                    method: 'GET'
	                })
	                    .done(resolve)
	                    .fail(reject);
	            });
	        }

	        post(url, data) {
	            return new Promise((resolve, reject) => {
	                $.ajax({
	                    method: 'POST',
	                    url: url,
	                    data: JSON.stringify(data),
	                    contentType: 'application/json'
	                })
	                    .done(resolve)
	                    .fail(reject);
	            });
	        }

	        put(url, data) {

	        }
	    }

	    return RequestsProvider;
	})();

	module.exports = RequestProvider;

/***/ },
/* 6 */
/***/ function(module, exports) {

	const DataService = (() => {
	    class DataService {
	        constructor(SERVER_URL, requestsProvider) {
	            this.URLS = SERVER_URL;

	            this.requests = requestsProvider;
	        }

	        isUserLoggedIn() {
	            return window.sessionStorage.USER_NAME && window.sessionStorage.AUTH_KEY;
	        }

	        loggedInUser() {
	            let username = 'annonymous';
	            if (this.isUserLoggedIn) {
	                username = window.sessionStorage.USER_NAME;
	            }

	            return username;
	        }

	        userLogin(user) {
	            const that = this;
	            return this.requests.post(this.URLS.USERS, user)
	                .then((response) => {
	                    that._saveUser(response);
	                    return response;
	                });
	        }

	        _saveUser(user) {
	            window.sessionStorage.setItem('USER_NAME', user.username);
	            window.sessionStorage.setItem('AUTH_KEY', user.authKey);
	        }
	    }

	    return DataService;
	})();

	module.exports = DataService;

/***/ },
/* 7 */
/***/ function(module, exports) {

	const HomeController = (() => {
	    class HomeController {
	        constructor() {

	        }

	        start(containerId) {
	            $(containerId).html('');
	        }
	    }

	    return HomeController;
	})();

	module.exports = HomeController;

/***/ }
/******/ ]);