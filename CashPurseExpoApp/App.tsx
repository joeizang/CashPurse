import { StatusBar } from 'expo-status-bar'
import { StyleSheet, Text, View, SafeAreaView, ScrollView } from 'react-native'
import { PaperProvider } from 'react-native-paper'
import { Colors } from 'react-native/Libraries/NewAppScreen'
import Main from './src/Main'
import { TabsProvider } from 'react-native-paper-tabs'
import { SafeAreaProvider } from 'react-native-safe-area-context'

export default function App() {
  return (
    <SafeAreaProvider>
      <SafeAreaView style={{ flex: 1 }}>
        <ScrollView style={{ flex: 1 }}>
          <Main />
        </ScrollView>
      </SafeAreaView>
    </SafeAreaProvider>
  );
}
