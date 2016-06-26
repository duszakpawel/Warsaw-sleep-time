var map;
var infowindow;

function initMap() {
    var myLatLng = { lat: 52.222202, lng: 21.006968 };

    var mapDiv = document.getElementById('minimap');
    map = new google.maps.Map(mapDiv, {
        center: myLatLng,
        zoom: 15
    });

    var marker = new google.maps.Marker({
        position: myLatLng,
        map: map,
        title: 'Warsaw Sleep Time',
        icon: 'https://maps.gstatic.com/mapfiles/ms2/micons/ltblue-dot.png'
    });
    google.maps.event.addListener(marker, 'click', function () {
        var contentString = '<div id="company">' +
            'Warsaw Sleep Time' + '<br />'
            + 'Koszykowa Street' + '<br />'
            + 'Warsaw' +
            '</div>';
        if (infowindow) {
            infowindow.close();
        }
        infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        infowindow.open(map, this);
    });

}