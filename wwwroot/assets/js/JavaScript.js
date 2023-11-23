//https://openweathermap.org/current

$('#weatherBtn').on('click', function () {
    var apiKey = 'f65be74d414bcceb8bee096a07ae99d9'

    var zip = document.getElementById('zip').value

    console.log(zip)

    $.ajax({
        url: `https://api.openweathermap.org/data/2.5/weather?zip=${zip},us&appid=${apiKey}&units=imperial`,
        method: 'GET',
        dataType: 'jsonp',
        success: function (data) {
            console.log(data)

            var output = document.getElementById('weatherOutput')

            output.innerHTML += `<div class="col-lg-12 p-3">
                                <div class="text-center"> 
                                    <h3>Weather - ${data.name}</h3>
                                    <p>${data.weather.map((x) => x.description)}</p>
                                    <p>Current Temp: ${data.main.temp}&deg;F</p>
                                    <p>Min Temp: ${data.main.temp_min}&deg;F</p> 
                                    <p>Max Temp: ${data.main.temp_max}&deg;F</p>
                                </div>
                            </div>`
        },
    })
})
