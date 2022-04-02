
let autocomplete;

/**
 * Initializes the autocomplete on an input element with  the ID: 'location'
 * */
function initAutocomplete() {
    autocomplete = new google.maps.places.Autocomplete(
        document.getElementById('location'),
        {
            types: ['establishment'],
            fields: ['place_id', 'geometry', 'name', 'formatted_address']
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
        const map = new google.maps.Map(document.getElementById('map'), {zoom: 14, center: place.geometry.location });
        const marker = new google.maps.Marker({ position: place.geometry.location, map: map });
    }
}