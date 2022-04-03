
let autocomplete;

/**
 * Initializes the autocomplete on an input element with  the ID: 'location'
 * */
function initAutocomplete() {
    autocomplete = new google.maps.places.Autocomplete(
        document.getElementById('location'),
        {
            types: ['establishment'],
            fields: ['place_id', 'geometry', 'name']
        });

    autocomplete.addListener('place_changed', onPlaceChanged);
}

/**
 * Callback for when an autocomplete option is clicked/changed
 * */
function onPlaceChanged() {
    var place = autocomplete.getPlace();

    if (!place.geometry) {
        document.getElementById('location').placeholder = 'Enter a place:';
    } else {
        const map = new google.maps.Map(document.getElementById('map'), {zoom: 20, center: place.geometry.location });
        const marker = new google.maps.Marker({ position: place.geometry.location, map: map });
    }
}

function initMap() {
    console.log("initMap");
    const location = document.getElementById("location").text;
    const geocoder = new google.maps.Geocoder();
    
    geocoder.geocode({ 'address': location },
        function(results, status) {
            if (status == google.maps.GeocoderStatus.OK && status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                console.log(results[0].geometry.location);
                const map = new google.maps.Map(document.getElementById('map'),
                    { zoom: 20, center: results[0].geometry.location });
                const marker = new google.maps.Marker({ position: results[0].geometry.location, map: map });
            } else {
                console.log(status);
            }
        });
}