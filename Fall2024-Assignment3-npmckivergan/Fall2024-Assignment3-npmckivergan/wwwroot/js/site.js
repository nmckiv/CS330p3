// Array of image paths
var images = [
    '/images/football_players/player1.jfif',
    '/images/football_players/player2.jfif',
    '/images/football_players/player3.jfif',
    '/images/football_players/player4.jfif',
    '/images/football_players/player5.jfif',
    '/images/football_players/player6.jfif',
    '/images/football_players/player7.jfif',
    '/images/football_players/player8.jfif',
    '/images/football_players/player9.jfif',
    '/images/football_players/player10.jfif',
    '/images/football_players/player11.jfif',
    '/images/football_players/player12.jfif',
    '/images/football_players/player13.jfif',
    '/images/football_players/player14.jfif',
    '/images/football_players/player15.jfif',
    '/images/football_players/player16.jfif',
    '/images/football_players/player17.jfif',
    '/images/football_players/player18.jfif',
    '/images/football_players/player19.jfif',
    '/images/football_players/player20.jfif',
    '/images/football_players/player21.jfif',
    '/images/football_players/player22.jfif',
    '/images/football_players/player23.jfif',
    '/images/football_players/player24.jfif',
    '/images/football_players/player25.jfif',
    '/images/football_players/player26.jfif',
    '/images/football_players/player27.jfif',
    '/images/football_players/player28.jfif'
];

// Function to pick a random image
function loadRandomImage() {
    const randomIndex = Math.floor(Math.random() * images.length);
    const imagePath = images[randomIndex]; // Directly use the path
    document.getElementById('randomPlayer').src = imagePath;
}

// Load a random image when the page loads
window.onload = loadRandomImage;
