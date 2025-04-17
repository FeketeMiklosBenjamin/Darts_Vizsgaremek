/// <reference types="Cypress"/>

describe("Bejövő üzenet tesztelése", () => {
    beforeEach(() => {
        cy.loginProfile('admin@admin.com', 'admin1234');
        cy.get('[data-cy="message_icon"]').click();
        cy.get('[data-cy="message"]').click({ multiple: true});
        cy.scrollTo('top');
    }) 
    
    it('Megjelenik az üzenet', () => {
        cy.get('[data-cy="message"]').should('be.visible');
        cy.get('[data-cy="message_title"]').should('have.text', 'Ez egy üzenet');
        cy.get('[data-cy="message_subject"]').should('have.text', 'Itt látható egy üzenet');
        cy.get('[data-cy="message_email_address"]').should('have.text', 'jancsika@gmail.com');
    })

    it('Visszanavigálás a főoldalra', () => {
        cy.get('[data-cy="back_to_main"]').click();
        cy.url().should('eq', 'http://localhost:5173/main-page')
    })

    // it('Üzenet törlése', () => {
    //     cy.get('[data-cy="delete_message"]').click();
    //     cy.wait(1000);
    //     cy.url().should('eq', 'http://localhost:5173/main-page');
    // })
})