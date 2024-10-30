$(document).ready(function () {
    $(".generate-review-btn").click(function (e) {
        e.preventDefault(); // Prevent the default form submission

        // Get the movie ID from a hidden input
        var movieId = $(this).data("movie-id");

        $.ajax({
            url: '/Actors/GenerateReview', // URL to the GenerateReview action
            type: 'POST',
            data: { movieId: movieId }, // Send the movie ID
            success: function (result) {
                // Optionally update the reviews table with the new review
                location.reload(); // Reload the page to reflect the new review
            },
            error: function (xhr, status, error) {
                alert("An error occurred: " + xhr.responseText);
            }
        });
    });
});