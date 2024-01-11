import { SafeAreaView, ScrollView } from 'react-native'
import Home from './components/screens/Home'
import { TabScreen, Tabs } from 'react-native-paper-tabs'
import BudgetList from './components/screens/BudgetList'
import Expense from './components/screens/Expense'
import { useState } from 'react'
import { BottomNavigation } from 'react-native-paper'



export default function Main() {
  const [index, setIndex] = useState(0)
  const [routes] = useState([
    { key: 'home', title: 'Home', focusedIcon: 'home' },
    { key: 'budget-list', title: 'Budget List', focusedIcon: 'book', unfocusedIcon: 'book-outline' },
    { key: 'expense', title: 'Expense', focusedIcon: 'note', unfocusedIcon: 'note-outline' }
  ])

  const renderScene = BottomNavigation.SceneMap({
    home: Home,
    'budget-list': BudgetList,
    expense: Expense
  })
  // const tabsProps = {
  //   uppercase: true,
  //   showTextLabel: true,
  //   style: { backgroundColor: 'slate' },
  //   mode: 'scrollable',
  //   showLeadingSpace: true,
  // }
  return (
        <BottomNavigation
          navigationState={{ index, routes }}
          onIndexChange={setIndex}
          renderScene={renderScene}
        />
  )
}