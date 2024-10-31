// Array of image paths
const images = [
    '/images/football_players/player1.jfif',
    '/images/football_players/player2.jfif',
    '/images/football_players/player3.jfif',
    '/images/football_players/player4.jfif'
    // Add more image paths as needed
];

// Function to pick a random image
function loadRandomImage() {
    const randomIndex = Math.floor(Math.random() * images.length);
    const imagePath = images[randomIndex]; // Directly use the path
    document.getElementById('randomPlayer').src = imagePath;
}

// Load a random image when the page loads
window.onload = loadRandomImage;
