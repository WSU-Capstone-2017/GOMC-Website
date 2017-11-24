/**
 * This creates the navspy jQuery plugin.  This uses the jQueryUI
 * widget factory to create the widget.
 *
 * Pass an object when invoking this that will set the options available.
 * Object properties are described in the parameters section.
 *
 * @param {Integer} top       Number of pixels to offset target activation from the top.
 * @param {Integer} bottom    Number of pixels to offset target activation from the bottom.
 * @param {Boolean} bottomOut Whether or not to activate the last item when at the bottom of the page.
 *
 * @see https://jqueryui.com/widget/
 */
$.widget('navspy', {

    options: {
        top: 0,
        bottom: 10,
        bottomOut: true
    },

    widgetEventPrefix: 'navspy:',

    active: null,
    paused: false,
    inBounds: false,
    max: null,
    min: null,
    last: null,

	/**
	 * Initialization function for the navspy jQuery plugin.
	 */
    _create: function () {
        this.element.uniqueId();
        this.uid = this.element.attr("id");

        this._setOptions({
            'top': this.options.top,
            'bottom': this.options.bottom,
            'bottomOut': this.options.bottomOut,
        });

        this._addListeners();
        this.refresh();
    },

	/**
	 * Saves the target menu items in an array.
	 *
	 * Since navspy is meant to be called on a set of menu items, this saves
	 * each menu item to an array.  Later each menu item will match an object
	 * containing the top and bottom positions of the div it is spying on.
	 */
    _setTargets: function () {
        var self = this
            , last = this.element.find("[data-spy-on]").length - 1

        this.element.find("[data-spy-on]").each(function (index, el) {
            var spy = "#" + $(this).attr("id")
                , spied = $(this).data("spy-on")

            if ($(spied).filter(":visible").length) {
                self.targets[spy] = {};
            }

            if (index == last) {
                self.last = spy;
            }
        });
    },

	/**
	 * Add event listeners to the document and window.
	 *
	 * A set of functions to run when on scroll and document resize.
	 *
	 * @todo See if the resize functions can be better applied to the targets.
	 */
    _addListeners: function () {
        var self = this
        $(window).on("scroll.navspy." + this.uid, function () {
            self._scrollCheck();
        });

        $(window).on("resize.navspy." + this.uid, function () {
            self.refresh();
        });
    },

	/**
	 * Removes the event listeners from the window and document.
	 *
	 * These are very importnat because since the event listeners are bound
	 * to thw window and document, they don't get cleaned up if the inital 
	 * element that invokes navspy gets removed from the DOM.  Without this
	 * we would see errors break javascript execution when the invoking element
	 * was removed.
	 */
    _destroy: function () {
        this._trigger("destroy", null, [this.element]);
        $(window).off("scroll.navspy." + this.uid);
        $(window).off("resize.navspy." + this.uid);
    },

	/**
	 * Sets the minimum scroll position and the maximum scroll position.
	 *
	 * Sets the minimum and maximum if the given sizes are larger or smaller than
	 * the existing minimum and maximum.  Min and Max are used to determine when
	 * the navspy should activate and deactivate.
	 * 
	 * @param {Object} size An object containing two y coordinates, top and bottom.
	 */
    _setMinMax: function (size) {
        if (size.top < this.min || this.min == null) {
            this.min = size.top;
        }
        if (size.bottom > this.max || this.max == null) {
            this.max = size.bottom;
        }
    },

	/**
	 * Sets the top and bottom bounds of individual target sections.
	 *
	 * This function sets the top and bottom scroll position of a target element, 
	 * and passes those positions to the _setMinMax() method so that it may check
	 * against the current minimum and maximum and add the new value if necessary.
	 */
    _setBounds: function () {
        for (var target in this.targets) {
            this.targets[target] = this._getSize(target)
            this._setMinMax(this.targets[target])
        }
    },

	/**
	 * Returns the top and bottom scroll position of an element.
	 * @param  {String} spy The jQuery selector string of the element.
	 * @return {Object}     An object with a top and bottom properies set 
	 * for the size of the given element.
	 */
    _getSize: function (spy) {
        var $section = $($(spy).data("spy-on"))
            , top = Math.floor($section.offset().top - this.options.top)
            , bottom = Math.floor(top + $section.outerHeight() - this.options.bottom)

        return { top: top, bottom: bottom }
    },

	/**
	 * Checks the current scroll position and activates/deactivates the appropriate
	 * target.
	 */
    _scrollCheck: function () {
        if (this.paused) {
            return false;
        }
        var scroll = $(window).scrollTop()

        if (!this._within(scroll, this.min, this.max)) {
            this.active && this.exit();
        } else {
            !this.inBounds && this.enter();
            if (!this.options.bottomOut || !this._bottomOut(scroll)) {
                for (var target in this.targets) {
                    var activated = this._maybeActivate(target, scroll);
                    if (activated) break;
                }
            }
        }
    },

	/**
	 * Activates a new target and deactivates an old target if the proper conditions
	 * are met.
	 * @param  {String}  target   The name of the target to check against.
	 * @param  {Integer} position The current scroll position.
	 */
    _maybeActivate: function (target, position) {
        if (this._within(position, this.targets[target].top, this.targets[target].bottom)) {
            return this._swapActive(target);
        }
        return false;
    },

	/**
	 * Determines if the last spy should activate due to page length limitations.
	 * @param  {Integer} position The current scroll position.
	 * @return {Boolean}          True if bottomed out, false if not.
	 */
    _bottomOut: function (position) {
        if (position + window.innerHeight >= $(document).height() && position < this.max) {
            return this._swapActive(this.last);
        }
        return false;
    },

	/**
	 * Takes a given target and activates it while deactivating the previously
	 * active target.
	 * @param  {String} target The name of the target to activate.
	 * @return {Boolean}       Returns true.
	 */
    _swapActive: function (target) {
        if (!this._isActive(target)) {
            this.deactivate(this.active)
            this.activate(target)
        }
        return true;
    },

	/**
	 * Determines if number is within range of two other numbers.
	 * @param  {integer} number The number to check, generally current scroll position.
	 * @param  {integer} min    The low end of the range to check.
	 * @param  {integer} max    The high end of the range to check.
	 * @return {boolean}        True if within range, false otherwise.
	 */
    _within: function (number, min, max) {
        return (min <= number) && (number <= max);
    },

	/**
	 * Whether or not the given target is currently active.
	 * @param  {String}  target The name of the target you want to check.
	 * @return {Boolean}        True if active, false if not.
	 */
    _isActive: function (target) {
        return (this.active === target);
    },

	/**
	 * Sets/Resets the navspy targets and bounds.
	 */
    refresh: function () {
        this.targets = {};
        this._setTargets();
        this._setBounds();
        this._scrollCheck();
    },

	/**
	 * Deactivates the currently active target, removes active class from 
	 * the target passed.
	 * @param  {String} link jQuery selector for the target to deactivate.
	 */
    deactivate: function (link) {
        this._trigger("deactivate", null, [$(link)]);
        this.element.find(".active").removeClass("active");
        this.active = null
    },

	/**
	 * Activates the target passed.
	 * @param  {String} link jQuery selector for the target to activate.
	 */
    activate: function (link) {
        this._trigger("activate", null, [$(link)]);
        $(link).addClass("active");
        this.active = link;
    },

	/**
	 * Triggers the enter event when scroll position enters the min/max range.
	 */
    enter: function () {
        this.inBounds = true;
        this._trigger("enter", null);
    },

	/**
	 * Triggers the exit event when scroll position exits the min/max range.
	 */
    exit: function () {
        this.deactivate(this.active)
        this.inBounds = false;
        this._trigger("exit", null);
    },

	/**
	 * Prevents anything from activating or deactivating, pauses positional checking.
	 */
    pause: function () {
        this.paused = true;
    },

	/**
	 * Resumes normal functionality after a pause.
	 */
    unpause: function () {
        this.paused = false;
        this._scrollCheck()
    },

})