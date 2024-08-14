function showConfirmation() {
    document.getElementById('deleteConfirmation').style.display = 'block';
}

function closeConfirmation() {
    document.getElementById('deleteConfirmation').style.display = 'none';
}

function deleteHome(id) {
    fetch(`/Homes/Delete/${id}`, {
        method: 'POST'
    })
        .then(response => {       
                window.location.href = '/Homes/Index';
        })
}

document.addEventListener('DOMContentLoaded', function () {
    var statesDropdown = document.getElementById('statesDropdown');
    var citiesDropdown = document.getElementById('citiesDropdown');

    statesDropdown.addEventListener('change', function () {
        var selectedState = this.value;
        if (selectedState) {
            fetch(`/Homes/GetCities?state=${selectedState}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    citiesDropdown.innerHTML = '<option value="">Select City</option>';
                    data.forEach(city => {
                        var option = document.createElement('option');
                        option.textContent = city;
                        option.value = city;
                        citiesDropdown.appendChild(option);
                    });
                })
                .catch(error => {
                    console.error('There was a problem with the fetch operation:', error);
                });
        } else {
            citiesDropdown.innerHTML = '<option value="">No Cities Were Found</option>';
        }
    });
});