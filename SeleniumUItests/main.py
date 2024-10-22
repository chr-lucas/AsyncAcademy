from tests import *

def runTest(test, name):
    result = test()
    if result[0] == True:
        print("Test passed :", name)
    else:
        print("TEST FAILED :", name)
        print(result[1])
        print(result[2])

runTest(testLogIn, "Logging In")
runTest(testAccountCreation, "Account Creation")
runTest(testGraphVisibility, "Graph Visibility for Student/Instructor Views")
