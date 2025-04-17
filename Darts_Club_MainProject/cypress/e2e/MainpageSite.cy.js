/// <reference types="Cypress"/>

describe("Főoldal tesztelése", () => {
    beforeEach(() => {
        cy.loginProfile('jancsika@gmail.com', 'jancsika');
        cy.scrollTo('top');
    })

    it('Megjelenik az oldal felhasználóval bejelentkezve', () => {
        cy.wait(1000);
        cy.url().should('eq', 'http://localhost:5173/main-page');
    })

    it('Nincsenek bejövő üzenetek', () => {
        cy.get('[data-cy="message_icon"]').click();
        cy.get('[data-cy="incoming_message"]').should('have.text', 'Nincs üzenete!');
        cy.get('[data-cy="message_icon"]').click();
    })

    it('Elnavigálás megtörténik', () => {
        cy.get('[data-cy="feedback_box"]').click();
        cy.url().should('eq', 'http://localhost:5173/feedback');
        cy.scrollTo('top');
        cy.get('[data-cy="home_icon"]').click();
        cy.url().should('eq', 'http://localhost:5173/main-page');
    })

    it('Kijelentkezés működik', () => {
        cy.get('[data-cy="logout_icon"]').click();
        cy.url().should('eq', 'http://localhost:5173/');
    })
})