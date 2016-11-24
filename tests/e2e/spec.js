describe('Protractor Demo App', function () {

    it('should have a title', function () {
        browser.get('http://localhost:3419/index.html#/signin');
        expect(browser.getTitle()).toEqual('Sign In');
    });

    it("signin to the page ", function () {
        var signin = element(by.model("signinModel.userName"));
        signin.sendkeys("Alex");

        var password = element(by.model("signinModel.password"));
        password.sendkeys("123456");

        var submitButton = element(by.id("btn-signin"));
        submitButton.click();

        
    });

});