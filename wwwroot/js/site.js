// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.addEventListener('DOMContentLoaded', () => {
  // Single prediction spinner
  const singleForm = document.getElementById('singleForm');
  const singleBtn  = document.getElementById('singleBtn');
  const singleSpin = document.getElementById('singleSpinner');
  singleForm.addEventListener('submit', () => {
    singleBtn.disabled = true;
    singleSpin.classList.remove('d-none');
  });

  // Batch scoring spinner
  const batchForm = document.getElementById('batchForm');
  const batchBtn  = document.getElementById('batchBtn');
  const batchSpin = document.getElementById('batchSpinner');
  batchForm.addEventListener('submit', () => {
    batchBtn.disabled = true;
    batchSpin.classList.remove('d-none');
  });
});
