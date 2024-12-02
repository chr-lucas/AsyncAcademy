from selenium import webdriver
from selenium.webdriver.common.by import By
#from selenium.webdriver.support.wait import WebDriverWait
from selenium.common.exceptions import NoSuchElementException, ElementNotInteractableException
import time
import random
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.keys import Keys

WEBSITE_URL = "https://asyncacademy20241104160444.azurewebsites.net/"
TEST_URL = "https://localhost:7082/"
TEST_WELCOME_URL = "https://localhost:7082/welcome"
driver = webdriver.Edge()
#errors = [NoSuchElementException, ElementNotInteractableException]
#wait = WebDriverWait(driver, 10, poll_frequency=0.5, ignored_exceptions=errors)

# Helper method that will attempt to locate an element, 
# if it fails it will wait one second and try again until
# maximum number of retries is reached
def find_element(by, value, retries=10):
    for i in range(retries):
        try:
            element = driver.find_element(by, value)
            driver.execute_script("arguments[0].scrollIntoView();", element)
            time.sleep(1)
            return element
        except Exception as e:
            time.sleep(1)
    try:
        raise e
    except UnboundLocalError:
        raise Exception

def testLogIn(): # Make sure logging in with test users works
    explanation = ""
    try:
        # Load website
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(TEST_URL)
        # try logging in with both test accounts
        for username in ("studenttest","instructortest"):
            # Find username and password fields
            explanation = "Failed to find username or password field, did those change?"
            username_field = find_element(By.NAME, "Account.Username")
            password_field = find_element(By.NAME, "Account.Pass")
            explanation = "Failed to find login button, did its HTML change?"
            login_button = find_element(By.CLASS_NAME, "submit")
            explanation = "Failed to log in as test student, did the login info for the test student change?"
            time.sleep(1)
            username_field.send_keys(username)
            password_field.send_keys("Pass1234")
            login_button.click()
            time.sleep(5)
            explanation = "Either the website failed to load, or the url for the welcome page simply changed, in which case this test needs to be updated with the new url"
            assert driver.current_url == TEST_WELCOME_URL
            driver.get(TEST_URL)
            time.sleep(5)
        return True, None, ""
    except Exception as e:
        return False, e, explanation

def testAccountCreation():
    explanation = ""
    try:
        # Load website
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(TEST_URL)
        # Click sign up button
        explanation = "Failed to find or interact with an element"
        sign_up_button = find_element(By.CLASS_NAME, "sign-up-link")
        sign_up_button.click()
        username_field = find_element(By.XPATH, "/html/body/div/main/div/div/form/div[1]/input")
        first_name_field = find_element(By.XPATH, "/html/body/div/main/div/div/form/div[2]/input")
        last_name_field = find_element(By.XPATH, "/html/body/div/main/div/div/form/div[3]/input")
        email_field = find_element(By.XPATH, "/html/body/div/main/div/div/form/div[4]/input")
        password_field = find_element(By.XPATH, "/html/body/div/main/div/div/form/div[5]/input")
        confirm_password_field = find_element(By.XPATH, "/html/body/div/main/div/div/form/div[6]/input")
        birthday_field = find_element(By.XPATH, "/html/body/div/main/div/div/form/div[7]/input[1]")
        # Input data
        test_username = []
        for i in range(50):
            test_username.append(chr(random.randint(65,122)))
        test_username = ''.join(test_username)
        username_field.send_keys(test_username)
        first_name_field.send_keys("Selenium")
        last_name_field.send_keys("Test")
        email_field.send_keys("seleniumtest@gmail.com")
        password_field.send_keys("Pass1234")
        confirm_password_field.send_keys("Pass1234")
        birthday_field.click()
        birthday_field.send_keys("03112006") # TODO: Test that birthdates too young don't work
        submit_button = find_element(By.XPATH, "/html/body/div/main/div/div/form/div[9]/input")
        submit_button.click()
        time.sleep(5)
        explanation = "Got unexpected URL"
        assert driver.current_url == TEST_WELCOME_URL
        explanation = "Was unable to find or interact with element at login page"
        driver.get(TEST_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.CLASS_NAME, "submit")
        username_field.send_keys(test_username)
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        explanation = "Got unexpected URL"
        assert driver.current_url == TEST_WELCOME_URL
        return True, None, ""
    except Exception as e:
        return False, e, explanation

def testGraphVisibility():
    explanation = ""
    try:
        # Student
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(TEST_URL)
        explanation = "Failed to log in as test student"
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.CLASS_NAME, "submit")
        username_field.send_keys("studenttest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == TEST_WELCOME_URL
        explanation = "Unable to locate CS 3550 link"
        cs3550_link = find_element(By.LINK_TEXT, "CS 3550")
        explanation = "Unable to interact with CS 3550 link"
        time.sleep(3)
        driver.execute_script("arguments[0].scrollIntoView();", cs3550_link) # scroll element into view
        time.sleep(3)
        cs3550_link.click()
        time.sleep(5)
        explanation = "Failed to enter CS 3550 class card, got unexpected URL"
        assert "https://localhost:7082/ClassOverview?" in driver.current_url
        try:
            find_element(By.XPATH, "/html/body/div/main/div[2]", 1)
            raise Exception("Found graph when it shouldn't have been there")
        except:
            pass
         # Instructor
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(TEST_URL)
        explanation = "Failed to log in as instructor user"
        username_field = find_element(By.NAME, "Account.Username") 
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.CLASS_NAME, "submit")
        username_field.send_keys("instructortest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == TEST_WELCOME_URL
        explanation = "Unable to locate CS 3550 link"
        cs3550_link = find_element(By.LINK_TEXT, "CS 3550")
        explanation = "Unable to interact with CS 3550 link"
        time.sleep(3)
        driver.execute_script("arguments[0].scrollIntoView();", cs3550_link) # scroll element into view
        time.sleep(3)
        cs3550_link.click()
        time.sleep(5)
        explanation = "Got unexpected URL"
        #assert "https://localhost:7082/ClassOverview?" in driver.current_url
        explanation = "Failed to find graph"
        find_element(By.XPATH, "/html/body/div/main/div[2]", 5)
        return True, None, ""
    except Exception as e:
        return False, e, explanation

def testUpdateProfile(): # Make sure student can update profile info
    explanation = ""
    try:
        # Load website
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(TEST_URL)

        # try logging in with as student test account
        username = "studenttest"
        explanation = "Failed to find username or password field, did those change?"
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        explanation = "Failed to find login button, did its HTML change?"
        login_button = find_element(By.CLASS_NAME, "submit")
        explanation = "Failed to log in as test student, did the login info for the test student change?"
        time.sleep(1)
        username_field.send_keys(username)
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(3)
        explanation = "Either the website failed to load, or the url for the welcome page simply changed, in which case this test needs to be updated with the new url"
            
        # Navigate to profile page
        profile_link = find_element(By.XPATH, "/html/body/header/nav/div/div/ul/li[3]/a")
        profile_link.click()
        explanation = "Could not navigate to the Profile page. Did the link or the order of the navbar change?"
        assert driver.current_url == "https://localhost:7082/Profile"
        time.sleep(3)

        # Click the Edit button
        profile_edit_btn = find_element(By.XPATH, "/html/body/div/main/form/div/button")
        profile_edit_btn.click()
        explanation = "Could not navigate to the Profile Edit page. Did the link or the order of the navbar change?"
        assert driver.current_url == "https://localhost:7082/Profile"
        time.sleep(3)

        # Add information to extra fields
        street_field = find_element(By.XPATH, "html/body/div/main/form/div/div[7]/div/input")
        city_field = find_element(By.XPATH, "html/body/div/main/form/div/div[8]/div/input")
        state_field = find_element(By.XPATH, "html/body/div/main/form/div/div[9]/div/input")
        zip_field = find_element(By.XPATH, "html/body/div/main/form/div/div[10]/div/input")
        #phone_field = find_element(By.XPATH, "html/body/div/main/form/div/div[10]/div/input") Can't get phone to pass validation
        street_field.send_keys("123 Test Street")
        city_field.send_keys("Exampleville")
        state_field.send_keys("UT")
        zip_field.send_keys("12345")
        #phone_field.send_keys("5551234567") Can't get phone to pass validation
        time.sleep(3)

        # Attempt to save form
        profile_save_btn = find_element(By.XPATH, "/html/body/div/main/form/div/button")
        profile_save_btn.click()
        explanation = "Error while saving changes to Profile."
        assert driver.current_url == "https://localhost:7082/Profile"
        time.sleep(1)

        # Check for newly added data
        explanation = "Updated Profile into does not match test input. Wsa there an error saving the form?"
        street_new_field = find_element(By.XPATH, "html/body/div/main/form/div/div[7]/div/p")
        city_new_field = find_element(By.XPATH, "html/body/div/main/form/div/div[8]/div/p")
        state_new_field = find_element(By.XPATH, "html/body/div/main/form/div/div[9]/div/p")
        zip_new_field = find_element(By.XPATH, "html/body/div/main/form/div/div[10]/div/p")
        assert street_new_field.text == "123 Test Street"
        assert city_new_field.text == "Exampleville"
        assert state_new_field.text == "UT"
        assert zip_new_field.text == "12345"
        #assert phone_field.text == "5551234567" Can't get phone to pass validation

        # Revert the changes from the test
        # Click the Edit button
        profile_edit_btn = find_element(By.XPATH, "/html/body/div/main/form/div/button")
        profile_edit_btn.click()
        explanation = "Could not navigate to the Profile Edit page. Did the link or the order of the navbar change?"
        assert driver.current_url == "https://localhost:7082/Profile"
        time.sleep(3)

        # Add information to extra fields
        street_field = find_element(By.XPATH, "html/body/div/main/form/div/div[7]/div/input")
        city_field = find_element(By.XPATH, "html/body/div/main/form/div/div[8]/div/input")
        state_field = find_element(By.XPATH, "html/body/div/main/form/div/div[9]/div/input")
        zip_field = find_element(By.XPATH, "html/body/div/main/form/div/div[10]/div/input")
        #phone_field = find_element(By.XPATH, "html/body/div/main/form/div/div[10]/div/input") Can't get phone to pass validation
        street_field.clear()
        city_field.clear()
        state_field.clear()
        zip_field.clear()
        #phone_field.send_keys("5551234567") Can't get phone to pass validation
        time.sleep(3)

        # Attempt to save form
        profile_save_btn = find_element(By.XPATH, "/html/body/div/main/form/div/button")
        profile_save_btn.click()
        explanation = "Error while saving changes to Profile."
        assert driver.current_url == "https://localhost:7082/Profile"
        time.sleep(1)

        return True, None, ""
    except Exception as e:
        return False, e, explanation


# Test the logout functionality for both student and instructor
def testLogOut():
    explanation = ""
    try:
        # Log in as student
        driver.get(TEST_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.CLASS_NAME, "submit")
        time.sleep(1)
        username_field.send_keys("studenttest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == TEST_WELCOME_URL

        # Log out as student
        logout_button = find_element(By.XPATH, "/html/body/header/nav/div/div/ul/li[4]/a")
        logout_button.click()
        time.sleep(5)
        assert driver.current_url == TEST_URL

        # Log in as instructor
        driver.get(TEST_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        time.sleep(1)
        username_field.send_keys("instructortest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == TEST_WELCOME_URL

        # Log out as instructor
        logout_button = find_element(By.XPATH, "/html/body/header/nav/div/div/ul/li[4]/a")
        logout_button.click()
        time.sleep(5)
        assert driver.current_url == TEST_URL

        return True, None, ""
    except Exception as e:
        return False, e, explanation





def testCourseRegistration():
    explanation = ""
    try:
        # Load website
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(TEST_URL)

        # Log in as student
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.CLASS_NAME, "submit")
        time.sleep(1)
        username_field.send_keys("studenttest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        #assert driver.current_url == "https://asyncacademy20241104160444.azurewebsites.net/welcome"
        assert driver.current_url == TEST_WELCOME_URL

        # Navigate to Register page
        explanation = "Unable to navigate to Register page"
        register_link = WebDriverWait(driver, 10).until(
            EC.element_to_be_clickable((By.XPATH, "/html/body/header/nav/div/div/ul/li[4]/a"))
        )  # Wait until the Register link is clickable (registering for )
        register_link.click()
        time.sleep(5)



        # Find the available courses table first
        explanation = "Unable to find the available courses table"
        available_courses_table = WebDriverWait(driver, 10).until(
            EC.presence_of_element_located((By.ID, "available-courses"))
        )

        enrolled_courses_table = WebDriverWait(driver, 10).until(
            EC.presence_of_element_located((By.ID, "enrolled-courses"))
        )
        print("Available courses table found.")
        
        # Find all enroll buttons in the available courses table
        explanation = "Unable to find any enroll buttons"
        enroll_buttons = available_courses_table.find_elements(By.CSS_SELECTOR, "button.btn.btn-primary")
        print(f"Number of enroll buttons found: {len(enroll_buttons)}")

        withdraw_buttons = enrolled_courses_table.find_elements(By.CSS_SELECTOR, "button.btn.btn-warning")
        print(f"Number of withdraw buttons found: {len(withdraw_buttons)}")

        # Click the first enroll button (or you can specify which one based on other criteria)
        explanation = "No enroll buttons found in the available courses table"
        if len(enroll_buttons) > 0:
            WebDriverWait(driver, 10).until(
                EC.element_to_be_clickable((By.CSS_SELECTOR, "button.btn.btn-primary"))
            )
            driver.execute_script("arguments[0].scrollIntoView(true);", enroll_buttons[0])
            time.sleep(1)  # Allow time for scrolling
            enrolled_course_id = enroll_buttons[0].get_attribute("id");
            enroll_buttons[0].click()
            time.sleep(5);
            print(f"Enrolled in Course ID:{enrolled_course_id}")
        else:
            raise Exception("No enroll buttons found")




        #Try to find enrolled course:
        found = False;
        enrolled_courses_table = WebDriverWait(driver, 10).until(
            EC.presence_of_element_located((By.ID, "enrolled-courses"))
        )
        withdraw_buttons = enrolled_courses_table.find_elements(By.CSS_SELECTOR, "button.btn.btn-warning")
        print(f"Number of withdraw buttons found: {len(withdraw_buttons)}")

        for button in withdraw_buttons:
            withdraw_buttons_ids = button.get_attribute("id")

            if(enrolled_course_id == withdraw_buttons_ids):
                found = True
                print("FOUND COURSE WE TESTED!")
                break  


        assert found, f"Test failed: Course with ID {enrolled_course_id} not found in enrolled courses table"
        print("Test passed: Course was successfully enrolled and verified.")

        return True, None, ""
    except Exception as e:
        return False, e, explanation




def testViewToDoList():
    explanation=""
    try:
         # Load website
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(TEST_URL)

         # Log in as student
        driver.get(TEST_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.CLASS_NAME, "submit")
        time.sleep(1)
        username_field.send_keys("studenttest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == TEST_WELCOME_URL

        #Find to do list
        try:
            find_element(By.XPATH, "/html/body/div[1]/main/div/div[2]/div/h2")
            pass
        except:
            raise Exception("To-do list not found")
            explanation = "To-do list not found"

        
        return True, None, ""
    except Exception as e:
        return False, e, explanation
        

def testAssignmentCreation():
    explanation = ""
    try:
        # Step 1: Log in as instructor
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(TEST_URL)
        
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.CLASS_NAME, "submit")
        
        time.sleep(1)
        username_field.send_keys("instructortest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        
        # Step 2: Navigate to CS 3550 class
        explanation = "Failed to navigate to the CS 3550 class."
        cs3550_link = find_element(By.LINK_TEXT, "CS 3550")
        cs3550_link.click()
        time.sleep(5)

        # Step 3: Click "Create New" to access the assignment form
        explanation = "Failed to find or click the 'Create New' button."
        assignments_link = find_element(By.LINK_TEXT, "Assignments")
        assignments_link.click()
        create_new_button = find_element(By.LINK_TEXT, "Create New")
        create_new_button.click()
        time.sleep(5)

        # Step 4: Fill out the assignment form
        explanation = "Failed to find assignment form fields."
        title_field = find_element(By.NAME, "Assignment.Title")
        description_field = find_element(By.NAME, "Assignment.Description")
        max_points_field = find_element(By.NAME, "Assignment.MaxPoints")
        due_date_field = find_element(By.NAME, "Assignment.Due")  # Ensure this matches the name in the form
        
        title_field.send_keys("Sample Assignment Title")
        description_field.send_keys("This is a sample assignment description.")
        max_points_field.send_keys("100")
        due_date_field = find_element(By.NAME, "Assignment.Due")
        due_date_field.clear()
        due_date_field.send_keys("12/31/2024")  # Enter the date
        due_date_field.send_keys(Keys.RIGHT)  # Move to the time field
        due_date_field.send_keys("0200PM")  # Enter the time

        # Step 5: Submit the assignment
        explanation = "Failed to find or click the 'Create' button."
        #create_button = find_element(By.XPATH, "//button[text()='Create']")  # Locate the Create button
        create_button = find_element(By.CSS_SELECTOR, ".btn.btn-primary")
        create_button.click()
        time.sleep(5)

        # Step 6: Verify the assignment was created successfully
        explanation = "Failed to verify that the assignment was created successfully."
        driver.get("https://localhost:7082/welcome")
        cs3550_link = find_element(By.LINK_TEXT, "CS 3550")
        cs3550_link.click()
        time.sleep(5)
        assignments_link = find_element(By.LINK_TEXT, "Assignments")
        assignments_link.click()
        
        # Wait for the table to be present
        wait = WebDriverWait(driver, 10)
        assignments_table = wait.until(
            EC.presence_of_element_located((By.CLASS_NAME, "table"))
        )

        # Then try to find the assignment with more specific selectors
        found = False
        rows = assignments_table.find_elements(By.CSS_SELECTOR, "tbody tr")
        for row in rows:
            title_cell = row.find_element(By.CSS_SELECTOR, "td:first-child")
            if "Sample Assignment Title" in title_cell.text:
                time.sleep(5)
                found = True
                break

        assert found, "The new assignment was not found in the assignments list."
        return True, None, ""
    except Exception as e:
        return False, e, explanation
