import type { Config } from 'jest';

const config: Config = {
  coverageThreshold: {
    global: {
      branches: 80,
      functions: 80,
      lines: 80,
      statements: 80,
    },
  },

  clearMocks: true,

  collectCoverage: true,

  coverageDirectory: 'coverage',

  coverageProvider: 'v8',

  // A preset that is used as a base for Jest's configuration
  preset: 'jest-preset-angular',

  setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],

  testEnvironment: 'jsdom',
};

export default config;
