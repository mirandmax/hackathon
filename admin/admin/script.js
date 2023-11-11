document.addEventListener("DOMContentLoaded", fetchData);

function fetchData() {
    // Replace 'your-api-endpoint' with your actual API endpoint
    fetch('http://172.20.10.18:5266/api/LKW/getTruckCount')
        .then(response => response.json())
        .then(data => {
            displayData(data);
        })
        .catch(error => {
            console.log("Error fetching data: ", error);
        });
}

function displayData(data) {
    const dataContainer = document.querySelector('.data-container');

    // Clear previous data
    dataContainer.innerHTML = '';

    // Display the fetched data in the admin panel
    data.forEach(item => {
        const dataItem = document.createElement('div');
        dataItem.classList.add('data-item');
        dataItem.innerHTML = `<p>ID: ${item.id}</p>
                              <p>Name: ${item.name}</p>
                              <p>Details: ${item.details}</p>`;
        dataContainer.appendChild(dataItem);
    });
}
