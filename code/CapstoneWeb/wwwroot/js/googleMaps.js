
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
        document.getElementById('map').innerHTML = place.name;
    }
}