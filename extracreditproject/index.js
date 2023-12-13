let myCars = []

async function handleOnLoad()
{   await updateCars()
    populateTable()
    let html=`
    <div id="tableBody"></div>
    <form onsubmit="return false">
    <div class ="tabover"><br>Add New Vehicle:</div><br>
        <label for="make" class = "tabover" >Car Make:</label><br>     
        <input type="text" id="make" name="make"class = "tabover" ><br>
        <label for="model" class = "tabover" >Car Model:</label><br>     
        <input type="text" id="model" name="model"class = "tabover" ><br>
        <label for="date" class = "tabover" >Date:</label><br>
        <input type="text" id="date" name="date" class = "tabover" placeholder = "YYYY-MM-DD"><br>
        <label for="mileage" class = "tabover" >Mileage:</label><br>
        <input type="text" id="mileage" name="mileage" class = "tabover" ><br><br>
        <button onclick="handleCarAdd()" class="submitbutton">Submit</button>
    </form>`
    document.getElementById('app').innerHTML=html
}
async function updateCars()
{
    let response = await fetch('http://localhost:5087/api/car' )
    myCars = await response.json()
}
async function populateTable()
{
    sortTableByDate();
    let html=`
    <div class="tablecontainer">
        <table class ="table table-striped">
        <tr>
            <th>Make</th>
            <th>Model</th>
            <th>Date</th>
            <th>Mileage</th>
            <th>Hold</th>
            <th>Delete</th>
        </tr>`
        myCars.forEach(function(car)
        {
            if(car.delete == false)
            {
                html += `
                <tr>
                <td>${car.make}</td>
                <td>${car.model}</td>
                <td>${car.date}</td>
                <td>${car.mileage}</td>
                `
                if(car.hold == false)
                {
                    html+=`<td><button class="btn " onclick="handleHold('${car.id}')">Not on Hold</button></td>`
                }
                else
                {
                    html+=`<td><button class="btn " onclick="handleHold('${car.id}')">On Hold</button></td>`
                }
                    html+=`<td><button class="btn btn-danger" onclick="handleCarDelete('${car.id}')">Delete</button></td>
                    </tr>`
            }
        })
        html+=`</table>
    </div>`
    document.getElementById('tableBody').innerHTML = html
}

async function handleCarAdd() 
{
    let carMake = document.getElementById('make').value;
    let carModel = document.getElementById('model').value;
    let carDate = document.getElementById('date').value;
    let carMileage = document.getElementById('mileage').value;
    
    const response = await fetch('http://localhost:5087/api/car', {
        method: 'POST', 
        body: JSON.stringify({
            Make: carMake,
            Model: carModel,
            Date: carDate,
            Mileage: carMileage,
        }),
        headers: 
        {
            'Content-Type': 'application/json'
        }
    });
    
    if(response.ok) {
        handleOnLoad();
        document.getElementById('make').value = '';
        document.getElementById('model').value = '';
        document.getElementById('date').value = '';
        document.getElementById('mileage').value = '';
    }
}


async function handleCarDelete(id)
{
    const response = await fetch('http://localhost:5087/api/car/'+ id, 
    {
        method: 'DELETE', 
        headers: 
        {
            'Content-Type': 'application/json'
        }
    })
    if(response.ok)
    {
        handleOnLoad();
    }
}

async function handleHold(id)
{
    console.log('toggling hold on id ', id)
    const car = myCars.find(car => car.id == id);
    console.log(car)
    car.hold = !car.hold;
    const response = await fetch('http://localhost:5087/api/car/'+ id, 
    {
        method: 'PUT', 
        body: JSON.stringify(car),
        headers: 
        {
            'Content-Type': 'application/json'
        }
    })
    if(response.ok)
    {
        populateTable();
    }
}

function sortTableByDate() 
{
    myCars.sort(function(a, b) {
        const dateA = new Date(a.date);
        const dateB = new Date(b.date);
        return dateA - dateB;
    });
}