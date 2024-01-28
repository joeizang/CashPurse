// import { LinearGradient } from 'expo-linear-gradient'
import { SafeAreaView, StatusBar, ScrollView } from 'react-native'
import React from 'react'
import ListExpense from './screens/Expense/ListExpense'
import CreateExpense from './screens/Expense/CreateExpense'

export default function App() {
  const colors = ['#2F4858', '#4b5563']
  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: '#2F4858' }}>
      <ScrollView style={{ flex: 1 }}>
        {/* <ListExpense /> */}
        <CreateExpense />
      </ScrollView>
      <StatusBar backgroundColor={colors[0]} />
    </SafeAreaView>
  )
}
