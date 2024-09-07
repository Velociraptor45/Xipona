const statusTexts = [
    "Collecting the bits",
    "Cleaning up the dishes",
    "Petting the cat",
    "Debugging the code",
    "Compiling the data",
    "Encrypting the files",
    "Updating the software",
    "Running the algorithm",
    "Backing up the database",
    "Syncing the devices",
    "Loading the assets",
    "Rendering the graphics",
    "Optimizing the performance",
    "Caching the content",
    "Deploying the updates",
    "Monitoring the servers",
    "Automating the tasks",
    "Securing the connection",
    "Scripting the commands",
    "Testing for bugs",
    "Organizing the pantry",
    "Stirring the pot",
    "Chopping the veggies",
    "Tasting the flavors",
    "Sifting through recipes",
    "Stocking up on ingredients",
    "Mixing it up",
    "Baking up a storm",
    "Prepping the meal",
    "Seasoning to perfection",
    "Garnishing with love",
    "Simmering with care",
    "Grating the cheese",
    "Whisking away",
    "Roasting to golden perfection",
    "Dicing and slicing",
    "Blending the flavors",
    "Setting the table",
    "Enjoying the feast"
]

let currentStatus = 0;
let intervalId = null;

function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}

function replaceContent(content) {
    const div = document.getElementById('status-rotation');
    div.innerHTML = content;
}

function doesDivExist() {
    return document.getElementById('status-rotation') !== null;
}

function rollStatus() {
    if (!doesDivExist()) {
        clearInterval(intervalId);
        return;
    }

    let nextStatus = currentStatus;

    while (nextStatus === currentStatus) {
        nextStatus = getRandomInt(statusTexts.length);
    }
    replaceContent(statusTexts[nextStatus]);
    currentStatus = nextStatus;
}

function startStatusRotation() {
    rollStatus();
    intervalId = setInterval(rollStatus, 1000);
}

startStatusRotation();