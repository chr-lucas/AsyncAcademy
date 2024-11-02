from selenium import webdriver
from selenium.webdriver.common.by import By
#from selenium.webdriver.support.wait import WebDriverWait
from selenium.common.exceptions import NoSuchElementException, ElementNotInteractableException
import time
import random

WEBSITE_URL = "https://asyncacademy20241005173644.azurewebsites.net/"
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
        driver.get(WEBSITE_URL)
        # try logging in with both test accounts
        for username in ("studenttest","instructortest"):
            # Find username and password fields
            explanation = "Failed to find username or password field, did those change?"
            username_field = find_element(By.NAME, "Account.Username")
            password_field = find_element(By.NAME, "Account.Pass")
            explanation = "Failed to find login button, did its HTML change?"
            login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
            explanation = "Failed to log in as test student, did the login info for the test student change?"
            time.sleep(1)
            username_field.send_keys(username)
            password_field.send_keys("Pass1234")
            login_button.click()
            time.sleep(5)
            explanation = "Either the website failed to load, or the url for the welcome page simply changed, in which case this test needs to be updated with the new url"
            assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"
            driver.get(WEBSITE_URL)
            time.sleep(5)
        return True, None, ""
    except Exception as e:
        return False, e, explanation

def testAccountCreation():
    explanation = ""
    try:
        # Load website
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(WEBSITE_URL)
        # Click sign up button
        explanation = "Failed to find or interact with an element"
        sign_up_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/a/button")
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
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"
        explanation = "Was unable to find or interact with element at login page"
        driver.get(WEBSITE_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        username_field.send_keys(test_username)
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        explanation = "Got unexpected URL"
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"
        return True, None, ""
    except Exception as e:
        return False, e, explanation

def testGraphVisibility():
    explanation = ""
    try:
        # Student
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(WEBSITE_URL)
        explanation = "Failed to log in as test student"
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        username_field.send_keys("studenttest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"
        explanation = "Unable to locate CS 3550 link"
        cs3550_link = find_element(By.LINK_TEXT, "CS 3550")
        explanation = "Unable to interact with CS 3550 link"
        time.sleep(3)
        driver.execute_script("arguments[0].scrollIntoView();", cs3550_link) # scroll element into view
        time.sleep(3)
        cs3550_link.click()
        time.sleep(5)
        explanation = "Failed to enter CS 3550 class card, got unexpected URL"
        assert "https://asyncacademy20241005173644.azurewebsites.net/ClassOverview?" in driver.current_url
        try:
            find_element(By.XPATH, "/html/body/div/main/div[2]", 1)
            raise Exception("Found graph when it shouldn't have been there")
        except:
            pass
         # Instructor
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(WEBSITE_URL)
        explanation = "Failed to log in as test student"
        username_field = find_element(By.NAME, "Account.Username") 
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        username_field.send_keys("instructortest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"
        explanation = "Unable to locate CS 3550 link"
        cs3550_link = find_element(By.LINK_TEXT, "CS 3550")
        explanation = "Unable to interact with CS 3550 link"
        time.sleep(3)
        driver.execute_script("arguments[0].scrollIntoView();", cs3550_link) # scroll element into view
        time.sleep(3)
        cs3550_link.click()
        time.sleep(5)
        explanation = "Got unexpected URL"
        #assert "https://asyncacademy20241005173644.azurewebsites.net/ClassOverview?" in driver.current_url
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
        driver.get(WEBSITE_URL)

        # try logging in with as student test account
        username = "studenttest"
        explanation = "Failed to find username or password field, did those change?"
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        explanation = "Failed to find login button, did its HTML change?"
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
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
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/Profile"
        time.sleep(3)

        # Click the Edit button
        profile_edit_btn = find_element(By.XPATH, "/html/body/div/main/form/div/button")
        profile_edit_btn.click()
        explanation = "Could not navigate to the Profile Edit page. Did the link or the order of the navbar change?"
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/Profile"
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
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/Profile"
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
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/Profile"
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
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/Profile"
        time.sleep(1)

        return True, None, ""
    except Exception as e:
        return False, e, explanation


# Test the logout functionality for both student and instructor
def testLogOut():
    explanation = ""
    try:
        # Log in as student
        driver.get(WEBSITE_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        time.sleep(1)
        username_field.send_keys("studenttest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"

        # Log out as student
        logout_button = find_element(By.XPATH, "/html/body/header/nav/div/div/ul/li[4]/a")
        logout_button.click()
        time.sleep(5)
        assert driver.current_url == WEBSITE_URL

        # Log in as instructor
        driver.get(WEBSITE_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        time.sleep(1)
        username_field.send_keys("instructortest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"

        # Log out as instructor
        logout_button = find_element(By.XPATH, "/html/body/header/nav/div/div/ul/li[4]/a")
        logout_button.click()
        time.sleep(5)
        assert driver.current_url == WEBSITE_URL

        return True, None, ""
    except Exception as e:
        return False, e, explanation





def testCourseRegistration():
    explanation=""
    try:
        # Load website
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(WEBSITE_URL)

        # Log in as student
        driver.get(WEBSITE_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        time.sleep(1)
        username_field.send_keys("studenttest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"


        #Navigate to Register page:
        register_link = find_element(By.XPATH, "/html/body/header/nav/div/div/ul/li[5]/a")
        register_link.click();
        explanation = "Unable to navigate to Register page"
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/Course%20Pages/StudentIndex"
        time.sleep(3);


        #Enroll in a course:
        enroll_button = find_element(By.XPATH, "/html/body/main/table[2]/tbody/tr[1]/td[14]/form/button")
        enroll_button.click();
        explanation = "Could not enroll in course"
        time.sleep(3);

        return True, None, ""
    except Exception as e:
        return False, e, explanation


def testViewToDoList():
    explanation=""
    try:
         # Load website
        explanation = "Failed to load website, are you connected to the internet? Is the website up? Did its URL change?"
        driver.get(WEBSITE_URL)

         # Log in as student
        driver.get(WEBSITE_URL)
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        time.sleep(1)
        username_field.send_keys("studenttest")
        password_field.send_keys("Pass1234")
        login_button.click()
        time.sleep(5)
        assert driver.current_url == "https://asyncacademy20241005173644.azurewebsites.net/welcome"

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
        driver.get(WEBSITE_URL)
        
        username_field = find_element(By.NAME, "Account.Username")
        password_field = find_element(By.NAME, "Account.Pass")
        login_button = find_element(By.XPATH, "/html/body/div/main/div/form/div[3]/div/input")
        
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
        create_new_button = find_element(By.LINK_TEXT, "Create New")
        create_new_button.click()
        time.sleep(5)

        # Step 4: Fill out the assignment form
        explanation = "Failed to find assignment form fields."
        title_field = find_element(By.NAME, "Title")
        description_field = find_element(By.NAME, "Description")
        max_points_field = find_element(By.NAME, "MaxPoints")
        due_date_field = find_element(By.NAME, "Due")  # Ensure this matches the name in the form
        
        title_field.send_keys("Sample Assignment Title")
        description_field.send_keys("This is a sample assignment description.")
        max_points_field.send_keys("100")
        due_date_field.send_keys("2024-12-31")  # Use an appropriate date format as required
        
        # Step 5: Submit the assignment
        explanation = "Failed to find or click the 'Create' button."
        create_button = find_element(By.XPATH, "//button[text()='Create']")  # Locate the Create button
        create_button.click()
        time.sleep(5)

        # Step 6: Verify the assignment was created successfully
        explanation = "Failed to verify that the assignment was created successfully."
        
        # Check if the assignment appears in the table of assignments
        assignments_table = find_element(By.TAG_NAME, "table")  # Find the assignments table
        rows = assignments_table.find_elements(By.TAG_NAME, "tr")  # Get all rows in the table
        
        # Extract titles from the table and check for our new assignment
        assignment_titles = []
        for row in rows[1:]:  # Skip the header row
            columns = row.find_elements(By.TAG_NAME, "td")
            if columns:  # Ensure there are columns
                title = columns[0].text  # The first column contains the title
                assignment_titles.append(title)

        assert "Sample Assignment Title" in assignment_titles, "The new assignment was not found in the assignments list."

        return True, None, ""
    except Exception as e:
        return False, e, explanation
