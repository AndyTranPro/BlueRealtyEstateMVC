function deleteHome(id) {
    fetch(`/Homes/Delete/${id}`, {
        method: 'POST'
    })
        .then(response => {
            window.location.href = '/Homes/Index';
        })
}