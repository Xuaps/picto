/*global pictoBackbone, Backbone*/

pictoBackbone.Collections = pictoBackbone.Collections || {};

(function () {
    'use strict';

    pictoBackbone.Collections.ApplicationCollection = Backbone.Collection.extend({

        model: pictoBackbone.Models.ApplicationModel

    });

})();
