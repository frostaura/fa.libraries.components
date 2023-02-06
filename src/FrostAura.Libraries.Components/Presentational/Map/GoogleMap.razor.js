window.faGoogleMap = {
    // Maps collection key/value pair.
    maps: {},
    // Markers collection key (map id) / value pair (collection of markers).
    markers: {},
    // Initialize a Google Map and set its zoom and center.
    initializeGoogleMapAsync: async (request) => {
        await window
            .faGoogleMap
            .boostrapMapAsync(request);
    },
    // Wire up the map in it's entirety async.
    boostrapMapAsync: async (request) => {
        await window.faGoogleMap.loadSdkAsync(request.apiKey);
        await window.faGoogleMap.initializeMapAsync(request);
    },
    // Load the Google Maps SDK script if it hasn't already been loaded.
    loadSdkAsync: (apiKey) => {
        const scriptId = 'googleMapsScript';
        const response = new Promise((resolve, reject) => {
            if (!!document.body.querySelector(`#${scriptId}`)) {
                resolve();

                return;
            }

            if (!apiKey) {
                reject('No api key was specified for component fa-google-map.');

                return;
            }

            const ele = document
                .createElement('script');

            ele.id = scriptId;
            ele.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}`;
            ele.onload = () => resolve();
            ele.onerror = () => reject();

            document.body.appendChild(ele);
        });

        return response;
    },
    // Initialize the map drivers.
    initializeMapAsync: async (request) => {
        const mapElement = document.querySelector('fa-google-map > div[id="' + request.id + '"]');

        if (!mapElement) throw new Error('No map element found.');

        await Promise.all([window.faGoogleMap.waitForComponentStylingAsync(request), window.faGoogleMap.waitForGoogleSdkAsync()]);

        const parentComponent = mapElement.parentElement;
        const width = parentComponent.offsetWidth;
        const height = parentComponent.offsetHeight;

        mapElement.style.width = `${width}px`;
        mapElement.style.height = `${height}px`;

        window.faGoogleMap.maps[request.id] = new google.maps.Map(mapElement, {
            center: { lat: parseFloat(request.center.lat), lng: parseFloat(request.center.lng) },
            zoom: request.zoom,
            mapTypeId: request.mapType,
            gestureHandling: 'cooperative',
            disableDefaultUI: true
        });
    },
    // Wait for the component's styling to kick in async.
    waitForComponentStylingAsync: async (request) => {
        const checkWidthRecursively = (resolve) => {
            const mapElement = document.querySelector('fa-google-map > div[id="' + request.id + '"]');
            const parentComponent = mapElement.parentElement;
            const width = parentComponent.offsetWidth;

            if (!width || width <= 0) {
                setTimeout(() => checkWidthRecursively(resolve));

                return;
            }

            resolve();
        };
        const response = new Promise((resolve) => {
            checkWidthRecursively(resolve);
        });

        return response;
    },
    // Wait for the Google SDK to kick in async.
    waitForGoogleSdkAsync: async () => {
        const ensureGoogleSdk = (resolve) => {
            const isGoogleSdkAvailable = !!window.google && !!window.google.maps && !!window.google.maps.Map;

            if (isGoogleSdkAvailable) {
                resolve();

                return;
            }

            setTimeout(() => ensureGoogleSdk(resolve));
        };
        const response = new Promise((resolve) => {
            ensureGoogleSdk(resolve);
        });

        return response;
    },
    // Add a marker to the map.
    addMarker: (mapId, request) => {
        window.faGoogleMap.markers[mapId] = window.faGoogleMap.markers[mapId] || [];

        const map = window.faGoogleMap.maps[mapId];
        const marker = new google.maps.Marker({
            position: request.location,
            title: request.title,
            map,
            icon: {
                url: request.icon.url,
                scaledSize: new google.maps.Size(request.icon.size, request.icon.size)
            }
        });
        const infoWindow = new google.maps.InfoWindow({});

        window.faGoogleMap.markers[mapId].push(marker);
        google.maps.event.addListener(marker, 'click', function () {
            infoWindow.setContent(request.info.content);
            infoWindow.open(window.faGoogleMap.maps[mapId], marker);
        });
    },
    // Clear all markers from the map.
    clearAllMarkers: (mapId) => {
        const markers = window.faGoogleMap.markers[mapId] = window.faGoogleMap.markers[mapId] || [];

        for (let i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }

        window.faGoogleMap.markers[mapId] = [];
    }
}