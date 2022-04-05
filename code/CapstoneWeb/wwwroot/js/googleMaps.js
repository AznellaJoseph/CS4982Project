
let autocomplete;

/**
 * Initializes the autocomplete on an input element with  the ID: 'location'
 * */
function initAutocomplete() {
    autocomplete = new google.maps.places.Autocomplete(
        document.getElementById("location"),
        {
            types: ["establishment"],
            fields: ["place_id", "geometry", "name"]
        });

    const geocoder = new google.maps.Geocoder();

    geocoder.geocode({ 'address': 'USA' },
        function (results, status) {
            if (status == google.maps.GeocoderStatus.OK && status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                const map = new google.maps.Map(document.getElementById("createmap"),
                    { zoom: 1, center: results[0].geometry.location });
            }
        });
    autocomplete.addListener("place_changed", onPlaceChanged);
}

/**
 * Callback for when an autocomplete option is clicked/changed
 * */
function onPlaceChanged() {
    const place = autocomplete.getPlace();

    if (!place.geometry) {
        document.getElementById("location").placeholder = "Enter a place:";
    } else {
        const map = new google.maps.Map(document.getElementById("createmap"), {zoom: 20, center: place.geometry.location });
        const marker = new google.maps.Marker({ position: place.geometry.location, map: map });
    }
}

function initMap() {
    const address = document.getElementById("location").innerHTML;
    const geocoder = new google.maps.Geocoder();
    
    geocoder.geocode({ 'address': address },
        function(results, status) {
            if (status == google.maps.GeocoderStatus.OK && status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                const map = new google.maps.Map(document.getElementById("map"),
                    { zoom: 20, center: results[0].geometry.location });
                const marker = new google.maps.Marker({ position: results[0].geometry.location, map: map });
            }
        });
}