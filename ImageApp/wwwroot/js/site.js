
// Event handler for keyup event
/*const handleKeyUp = (event) => {
    const email = event.target.value;
    checkEmailExistence(email);
};

// Attach event handler to email input
document.getElementById('emailInput').addEventListener('keyup', handleKeyUp);


const checkEmailExistence = async (email) => {
    try {
        const response = await axios.get('https://api.zerobounce.net/v2/validate', {
            params: {
                api_key: '52c5031d97f84fc5931c5789d23c127d',
                email: email
            }
        });

        const { status } = response.data;

        // Check if email exists
        if (status === 'valid') {
            document.getElementById('errorMessage').innerText = '';
        } else {
            document.getElementById('errorMessage').innerText = 'Email does not exist';
        }
    } catch (error) {
        console.error(error);
    }
};*/

<script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>

const form = document.querySelector('myForm');
const emailInput = document.querySelector('#emailInput');
const errorMessage = document.querySelector('#errorMessage');

// Function to check email existence
const checkEmailExistence = async (email) => {
    try {
        const response = await axios.get('https://api.zerobounce.net/v2/validate?api_key=52c5031d97f84fc5931c5789d23c127d&email=jsonosii097@gmail.com', {
            
        });

        const { status } = response.data;

        // Check if email exists
        if (status === 'valid') {
            errorMessage.innerText = '';
        } else {
            errorMessage.innerText = 'Email does not exist';
        }
    } catch (error) {
        console.error(error);
    }
};

// Event handler for keyup event
const handleKeyUp = (event) => {
    const email = event.target.value;
    checkEmailExistence(email);
};

// Event handler for form submission
const handleSubmit = (event) => {
    event.preventDefault();
    const email = emailInput.value;
    checkEmailExistence(email);

    // Check if email is valid before submitting the form
    if (errorMessage.innerText !== '') {
        return;
    }

    // Proceed with form submission
    form.submit();
};

// Attach event handlers
emailInput.addEventListener('keyup', handleKeyUp);
form.addEventListener('submit', handleSubmit);
};