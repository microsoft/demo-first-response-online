var CanvasHeatmap = function (canvasElm, alphaMask, options) {
    /* Private Properties */
    var _canvas,
      _temperaturemap,
      _locations = [],
      _threadID = 0;
    
    // Set default options
    var _options = {
        // Opacity at the centre of each heat point
        intensity: 0.1,

        // Affected radius of each heat point in pixels
        radius: 10,

        zoom: 1,

        hardEdgeAlpha : 0.5,

        // Colour temperature gradient of the map
        colorgradient: {
            "0.00": 'rgba(0,0,128,255)',  // Navy
            "0.25": 'rgba(0,0,255,255)',    // Blue
            "0.50": 'rgba(0,255,0,255)',    // Green
            "0.75": 'rgba(255,255,0,255)', // Yellow
            "1.00": 'rgba(255,0,0,255)'    // Red
        },
        callback : null
    };

    var bgWorker = null;

    /* Private Methods */
    function _init() {
        _canvas = canvasElm;
        _temperaturemap = _createColourGradient(_options.colorgradient);

        // Override defaults with any options passed in the constructor
        _setOptions(options);

        delete _init;
    }

    // Creates a color gradient from supplied color stops on initialisation
    function _createColourGradient(colorstops) {
        var ctx = document.createElement('canvas').getContext('2d');
        var grd = ctx.createLinearGradient(0, 0, 256, 0);
        for (var c in colorstops) {
            grd.addColorStop(c, colorstops[c]);
        }
        ctx.fillStyle = grd;
        ctx.fillRect(0, 0, 256, 1);
        return ctx.getImageData(0, 0, 256, 1).data;
    }

    // Sets any options passed in
    function _setOptions(options) {
        var grdChg = false;
        for (attrname in options) {
            _options[attrname] = options[attrname];
            grdChg |= (attrname == 'colorgradient');
        }

        // Create a color gradient from the suppied colorstops
        if (grdChg) {
            _temperaturemap = _createColourGradient(_options.colorgradient);
        }
    }

    // Main method to draw the heatmap
    function _createHeatMap(points) {

        if (points) {
            _locations = [];
        }

        //Terminate any on going processing to release resources.
        if (bgWorker) {
            bgWorker.terminate();
            bgWorker = null;
        }

        var ctx = _canvas.getContext("2d");
        ctx.clearRect(0, 0, _canvas.width, _canvas.height);



        if (_options.radius >= 1) {

            //Set Intensity
            ctx.globalAlpha = _options.intensity;

            var mapWidth = Math.round(256 * Math.pow(2, _options.zoom));

            var drawMultiple = (mapWidth < _canvas.width) && (_options.zoom <= 3);

            var x, x2, y, nums,
                left = -_options.radius,
                right = _canvas.width + _options.radius,
                top = -_options.radius,
                bottom = _canvas.height + _options.radius;

            // Create the Intensity Map by looping through each location
            if (points) {
                var pairs = points.split("|");

                for (var i = 0, len = pairs.length; i < len; i++) {
                    nums = pairs[i].split(",");
                    x = parseInt(nums[0]);
                    y = parseInt(nums[1]);
                    _locations.push({ 'x': x, 'y': y });

                    if (x < 0) {
                        x += mapWidth * Math.ceil(Math.abs(x / mapWidth));
                    }

                    _drawPoint(ctx, x, y, left, right, top, bottom);

                    if (drawMultiple) {
                        if (x > mapWidth) {
                            //draw left
                            x2 = x - mapWidth;
                            _drawPoint(ctx, x2, y, left, right, top, bottom);
                        } else {
                            //draw right
                            x2 = x + mapWidth;
                            _drawPoint(ctx, x2, y, left, right, top, bottom);
                        }
                    }
                }
            } else {
                for (var i = 0, len = _locations.length; i < len; i++) {
                    x = _locations[i].x;
                    y = _locations[i].y;

                    if (x < 0) {
                        x += mapWidth * Math.ceil(Math.abs(x / mapWidth));
                    }

                    _drawPoint(ctx, x, y, left, right, top, bottom);

                    if (drawMultiple) {
                        if (x > mapWidth) {
                            //draw left
                            x2 = x - mapWidth;
                            _drawPoint(ctx, x2, y, left, right, top, bottom);
                        } else {
                            //draw right
                            x2 = x + mapWidth;
                            _drawPoint(ctx, x2, y, left, right, top, bottom);
                        }
                    }
                }
            }

            // Apply the specified color gradient to the intensity map
            var dat = ctx.getImageData(0, 0, _canvas.width, _canvas.height);

            // If workers are not supported
            // Perform all calculations in current thread as usual
            if (!window.Worker) {

                var pix = dat.data; // pix is a CanvasPixelArray containing height x width x 4 bytes of data (RGBA)
                var len = pix.length;
                var alpha;

                if (_options.enableHardEdge) {
                    var alphaOverride = 256 * _options.intensity;

                    for (var p = 0; p < len; p += 4) {
                        alpha = pix[p + 3] * 4; // get the alpha of this pixel
                        if (alpha != 0) { // If there is any data to plot
                            pix[p] = _temperaturemap[alpha]; // set the red value of the gradient that corresponds to this alpha
                            pix[p + 1] = _temperaturemap[alpha + 1]; //set the green value based on alpha
                            pix[p + 2] = _temperaturemap[alpha + 2]; //set the blue value based on alpha
                            pix[p + 3] = alphaOverride;
                        }
                    }
                } else {

                    for (var p = 0; p < len; p += 4) {
                        alpha = pix[p + 3] * 4; // get the alpha of this pixel
                        if (alpha != 0) { // If there is any data to plot
                            pix[p] = _temperaturemap[alpha]; // set the red value of the gradient that corresponds to this alpha
                            pix[p + 1] = _temperaturemap[alpha + 1]; //set the green value based on alpha
                            pix[p + 2] = _temperaturemap[alpha + 2]; //set the blue value based on alpha
                        }
                    }
                }

                ctx.putImageData(dat, 0, 0);

                if (_options.callback) {
                    var imgUri = _canvas.toDataURL("image/png");
                    _options.callback(imgUri);
                }
            } else {
                //Use Web Worker to multithread the colorization process
                //Sending canvas data to the worker using a copy memory operation
                //bgWorker = new Worker("ColorizeWebWorker.js");

                var blob = new Blob([ '(', 
                	function()  {
                	// init

                	self.onmessage = function (e) {
					    var canvasData = e.data.data;
					    var pix = canvasData.data;

					    var len = pix.length;
					    var enableHardEdge = e.data.enableHardEdge;
					    var intensity = e.data.intensity;
					    var temperaturemap = e.data.temperaturemap;

					    if (enableHardEdge) {
					        var alphaOverride = 256 * intensity;
					        var alpha;

					        for (var p = 0; p < len; p += 4) {
					            alpha = pix[p + 3] * 4; // get the alpha of this pixel
					            if (alpha != 0) { // If there is any data to plot
					                pix[p] = temperaturemap[alpha]; // set the red value of the gradient that corresponds to this alpha
					                pix[p + 1] = temperaturemap[alpha + 1]; //set the green value based on alpha
					                pix[p + 2] = temperaturemap[alpha + 2]; //set the blue value based on alpha
					                pix[p + 3] = alphaOverride;
					            }
					        }
					    } else {
					        for (var p = 0; p < len; p += 4) {
					            alpha = pix[p + 3] * 4; // get the alpha of this pixel
					            if (alpha != 0) { // If there is any data to plot
					                pix[p] = temperaturemap[alpha]; // set the red value of the gradient that corresponds to this alpha
					                pix[p + 1] = temperaturemap[alpha + 1]; //set the green value based on alpha
					                pix[p + 2] = temperaturemap[alpha + 2]; //set the blue value based on alpha
					            }
					        }
					    }

					    self.postMessage({ result: canvasData, id: e.data.id });
					};
                	// end
                	}.toString(),
                ')()' ], { type: 'application/javascript'});

                var blobUrl = URL.createObjectURL(blob);
                bgWorker = new Worker(blobUrl);
                //bgWorker = new Worker("ColorizeWebWorker.js");

                bgWorker.onmessage = _colorizeCompleted;
                _threadID++;
                bgWorker.postMessage({ data: dat, intensity: _options.intensity, enableHardEdge: _options.enableHardEdge, temperaturemap: _temperaturemap, id: _threadID });
            }
        }
    }

    function _drawPoint(ctx, x, y, left, right, top, bottom) {
        if (x > left && x < right && y > top && y < bottom) {
            ctx.drawImage(alphaMask, 0, 0, 512, 512, x - _options.radius, y - _options.radius, 2 * _options.radius, 2 * _options.radius);
        }
    }

    function _colorizeCompleted(e) {
        if (e.data.id == _threadID) {
            var ctx = _canvas.getContext("2d");
            ctx.putImageData(e.data.result, 0, 0);

            if (_options.callback) {
                var imgUri = _canvas.toDataURL("image/png");
                _options.callback(imgUri);
            }
        }
    }

    /* Public Methods */

    this.Render = function (points, opt) {
        if (opt) {
            _setOptions(opt);
        }

        _createHeatMap(points);
    };

    this.TerminateWorker = function () {
        if (bgWorker) {
            bgWorker.terminate();
            bgWorker = null;
        }
    };

    // Call the initialisation routine
    _init();
};